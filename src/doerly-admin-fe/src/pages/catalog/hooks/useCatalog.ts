import { useEffect, useMemo, useRef, useState } from 'react';
import apiCatalog from 'api/catalog/catalog';
import { IGetCategoryResponse } from 'api/catalog/model';
import { IEditableCategory } from 'pages/catalog/model/category';

const errorTypes = {
  EMPTY_FIELD_DESCRIPTION: 'EMPTY_FIELD_DESCRIPTION',
  EMPTY_FIELD_NAME: 'EMPTY_FIELD_NAME',
};

const categoryFields: { [key: string]: string } = {
  DESCRIPTION: 'description',
  IS_ENABLED: 'isEnabled',
  NAME: 'name',
  PARENT_ID: 'parentId',
};

const fieldsToProperties: Record<string, keyof IEditableCategory> = {
  [categoryFields.DESCRIPTION]: 'description',
  [categoryFields.IS_ENABLED]: 'isEnabled',
  [categoryFields.NAME]: 'name',
  [categoryFields.PARENT_ID]: 'parentId',
};


const useCatalog = () => {

  const componentDidMount = useRef(false);

  const [containerState, setContainerState] = useState({
    categoryIdToDelete: null as number | null,
    editableCategoryId: null as number | null,
    showAfterCreateCategorySuccessAlert: false,
    showAfterDeleteCategorySuccessAlert: false,
    showAfterUpdateCategorySuccessAlert: false,
    showCreateUpdateDialog: false,
    showDeletedCategories: false,
    showDisabledCategories: false,
    validationErrors: [] as string[],
  });
  
  const defaultEditableCategory: IEditableCategory = {
    description: '',
    isEnabled: true,
    name: '',
    parentId: null,
  };
  
  const [editableCategory, setEditableCategory] =
    useState<IEditableCategory>(defaultEditableCategory);

  const [categories, setCategoriesState] = useState<ICategoriesState>({
    isFailed: false,
    isFailedCreate: false,
    isFailedDelete: false,
    isFailedUpdate: false,
    isFetching: false,
    isFetchingCreate: false,
    isFetchingDelete: false,
    isFetchingUpdate: false,
    list: [] as Array<IGetCategoryResponse> | null,
  });

  const filteredCategories = useMemo(() => {
    if (!categories.list) return null;

    const filterCategories = (
      categoriesList: IGetCategoryResponse[]
    ): IGetCategoryResponse[] => {
      return categoriesList
        .filter(category => {
          if (!containerState.showDisabledCategories && !category.isEnabled) {
            return false;
          }
          if (!containerState.showDeletedCategories && category.isDeleted) {
            return false;
          }
          return true;
        })
        .map(category => ({
          ...category,
          children: category.children
            ? filterCategories(category.children)
            : [],
        }));
    };

    return filterCategories(categories.list);
  }, [
    categories.list,
    containerState.showDisabledCategories,
    containerState.showDeletedCategories,
  ]);


  const isFetchingSave = categories.isFetchingCreate
    || categories.isFetchingUpdate;

  const fetchCategories = () => {
    apiCatalog.getCategories({
      onFailed: () => setCategoriesState((prevState) => ({
        ...prevState,
        isFailed: true,
        isFetching: false,
      })),
      onRequest: () => setCategoriesState((prevState) => ({
        ...prevState,
        isFailed: false,
        isFetching: true,
      })),
      onSuccess: ({ value }: any) => setCategoriesState((prevState) => ({
        ...prevState,
        isFailed: false,
        isFetching: false,
        list: value as Array<IGetCategoryResponse>,
      })),
    });
  };

  const getParentId = (childId: number): number | null => {
    const searchInCategories = (
      categoriesList: IGetCategoryResponse[],
      targetId: number,
      parentId: number | null = null
    ): number | null => {
      for (const category of categoriesList) {
        if (category.id === targetId) {
          return parentId;
        }

        if (category.children && category.children.length > 0) {
          const foundParentId = searchInCategories(
            category.children,
            targetId,
            category.id
          );
          if (foundParentId !== null) {
            return foundParentId;
          }
        }
      }

      return null;
    };

    return searchInCategories(categories.list || [], childId);
  };

  const getCategoryById = (
    categoryId: number
  ): IGetCategoryResponse | null => {
    const findCategory = (
      categoriesList: IGetCategoryResponse[]
    ): IGetCategoryResponse | null => {
      for (const category of categoriesList) {
        if (category.id === categoryId) {
          return category;
        }
        if (category.children && category.children.length > 0) {
          const foundCategory = findCategory(category.children);
          if (foundCategory) {
            return foundCategory;
          }
        }
      }
      return null;
    };
    return findCategory(categories.list || []) || null;
  };

  const getValidationErrors = () => {
    const errors = [];
    if (!editableCategory.name.trim()) {
      errors.push(errorTypes.EMPTY_FIELD_NAME);
    }
    if (!editableCategory.description.trim()) {
      errors.push(errorTypes.EMPTY_FIELD_DESCRIPTION);
    }
    return errors;
  };

  const handleCloseAfterCreateCategorySuccessAlert = () => {
    setContainerState(prevState => ({
      ...prevState,
      showAfterCreateCategorySuccessAlert: false,
    }));
  };

  const handleCloseAfterDeleteCategorySuccessAlert = () => {
    setContainerState(prevState => ({
      ...prevState,
      showAfterDeleteCategorySuccessAlert: false,
    }));
  };

  const handleCloseAfterUpdateCategorySuccessAlert = () => {
    setContainerState(prevState => ({
      ...prevState,
      showAfterUpdateCategorySuccessAlert: false,
    }));
  };

  const handleCloseCreateUpdateDialog = () => {
    setContainerState({
      ...containerState,
      showCreateUpdateDialog: false,
    });
  };
  
  const handleCloseDeleteDialog = () => {
    setContainerState((prevState) => ({
      ...prevState,
      categoryIdToDelete: null,
    }));
  };

  const handleConfirmDeleteCategory = () => {
    apiCatalog.deleteCategory({
      categoryId: containerState.categoryIdToDelete as number,
      onFailed: () => setCategoriesState((prevState) => ({
        ...prevState,
        isFailed: true,
        isFetchingDelete: false,
      })),
      onRequest: () => setCategoriesState((prevState) => ({
        ...prevState,
        isFailed: false,
        isFetchingDelete: true,
      })),
      onSuccess: () => setCategoriesState((prevState) => ({
        ...prevState,
        isFetchingDelete: false,
      })),
    });
  };

  const handleConfirmSaveCategory = () => {
    const validationErrors = getValidationErrors();
    if (!validationErrors.length) {
      if (containerState.editableCategoryId) {
        apiCatalog.updateCategory({
          category: editableCategory,
          categoryId: containerState.editableCategoryId,
          onFailed: () => setCategoriesState((prevState) => ({
            ...prevState,
            isFailedUpdate: true,
            isFetchingUpdate: false,
          })),
          onRequest: () => setCategoriesState((prevState) => ({
            ...prevState,
            isFailedUpdate: false,
            isFetchingUpdate: true,
          })),
          onSuccess: () => setCategoriesState((prevState) => ({
            ...prevState,
            isFetchingUpdate: false,
          })),
        });
      } else {
        apiCatalog.createCategory({
          category: editableCategory,
          onFailed: () => setCategoriesState((prevState) => ({
            ...prevState,
            isFailedCreate: true,
            isFetchingCreate: false,
          })),
          onRequest: () => setCategoriesState((prevState) => ({
            ...prevState,
            isFailedCreate: false,
            isFetchingCreate: true,
          })),
          onSuccess: () => setCategoriesState((prevState) => ({
            ...prevState,
            isFetchingCreate: false,
          })),
        });
      }
    }
    setContainerState({
      ...containerState,
      validationErrors,
    });
  };

  const handleEditableFieldChange = ({
    fieldKey,
    value,
  }: any) => {
    setEditableCategory((prevState => ({
      ...prevState,
      [fieldsToProperties[fieldKey]]: value,
    })));
  };

  const handleStartCreate = (parentId: number) => {
    setEditableCategory({
      ...defaultEditableCategory,
      parentId: parentId || null,
    });
    setContainerState((prevState) => ({
      ...prevState,
      showCreateUpdateDialog: true,
    }));
  };

  const handleStartDelete = (categoryId: number) => {
    setContainerState((prevState) => ({
      ...prevState,
      categoryIdToDelete: categoryId,
    }));
  };

  const handleStartEdit = (category: IGetCategoryResponse) => {
    console.log(getParentId(category.id));
    setEditableCategory({
      description: category.description || '',
      isEnabled: category.isEnabled,
      name: category.name,
      parentId: getParentId(category.id),
    });
    setContainerState((prevState) => ({
      ...prevState,
      editableCategoryId: category.id,
      showCreateUpdateDialog: true,
    }));
  };

  const handleToggleEnabled = (categoryId: number, isEnabled: boolean) => {
    const editableCategory = getCategoryById(categoryId);
    const parentId = getParentId(categoryId);
    apiCatalog.updateCategory({
      category: {
        ...editableCategory,
        isEnabled: isEnabled,
        parentId: parentId || null,
      },
      categoryId,
      onFailed: () => setCategoriesState((prevState) => ({
        ...prevState,
        isFailedUpdate: true,
        isFetchingUpdate: false,
      })),
      onRequest: () => setCategoriesState((prevState) => ({
        ...prevState,
        isFailedUpdate: false,
        isFetchingUpdate: true,
      })),
      onSuccess: () => setCategoriesState((prevState) => ({
        ...prevState,
        isFetchingUpdate: false,
      })),
    });
  };

  const handleToggleShowDeletedCategories = () => {
    setContainerState(prevState => ({
      ...prevState,
      showDeletedCategories: !prevState.showDeletedCategories,
    }));
  };

  const handleToggleShowDisabledCategories = () => {
    setContainerState(prevState => ({
      ...prevState,
      showDisabledCategories: !prevState.showDisabledCategories,
    }));
  };

  useEffect(() => {
    if (componentDidMount.current
      && !categories.isFetchingDelete
      && !categories.isFailedDelete
    ) {
      setContainerState(prevState => ({
        ...prevState,
        categoryIdToDelete: null,
        showAfterDeleteCategorySuccessAlert: true,
      }));
    }
  }, [categories.isFetchingDelete]);

  useEffect(() => {
    if (componentDidMount.current
      && !categories.isFetchingCreate
      && !categories.isFailedCreate
    ) {
      setContainerState(prevState => ({
        ...prevState,
        showAfterCreateCategorySuccessAlert: true,
        showCreateUpdateDialog: false,
      }));
      setEditableCategory(defaultEditableCategory);
    }
  }, [categories.isFetchingCreate]);

  useEffect(() => {
    if (componentDidMount.current
      && !categories.isFetchingUpdate
      && !categories.isFailedUpdate
    ) {
      setContainerState(prevState => ({
        ...prevState,
        editableCategoryId: null,
        showAfterUpdateCategorySuccessAlert: true,
        showCreateUpdateDialog: false,
      }));
      setEditableCategory(defaultEditableCategory);
    }
  }, [categories.isFetchingUpdate]);

  useEffect(() => {
    if (componentDidMount.current
      && !categories.isFailedCreate
      && !categories.isFailedDelete
      && !categories.isFailedUpdate
      && !categories.isFetchingCreate
      && !categories.isFetchingDelete
      && !categories.isFetchingUpdate
    ) {
      fetchCategories();
    }
  }, [
    categories.isFetchingCreate,
    categories.isFetchingDelete,
    categories.isFetchingUpdate,
  ]);

  useEffect(() => {
    componentDidMount.current = true;
    fetchCategories();
  }, []);

  return {
    categories: filteredCategories,
    categoryFields,
    containerState,
    editableCategory,
    handleCloseAfterCreateCategorySuccessAlert,
    handleCloseAfterDeleteCategorySuccessAlert,
    handleCloseAfterUpdateCategorySuccessAlert,
    handleCloseCreateUpdateDialog,
    handleCloseDeleteDialog,
    handleConfirmDeleteCategory,
    handleConfirmSaveCategory,
    handleEditableFieldChange,
    handleStartCreate,
    handleStartDelete,
    handleStartEdit,
    handleToggleEnabled,
    handleToggleShowDeletedCategories,
    handleToggleShowDisabledCategories,
    isFailed: categories.isFailed,
    isFetching: categories.isFetching,
    isFetchingDelete: categories.isFetchingDelete,
    isFetchingSave,
    showAfterCreateCategorySuccessAlert:
      containerState.showAfterCreateCategorySuccessAlert,
    showAfterDeleteCategorySuccessAlert:
      containerState.showAfterDeleteCategorySuccessAlert,
    showAfterUpdateCategorySuccessAlert:
      containerState.showAfterUpdateCategorySuccessAlert,
  };
};

interface ICategoriesState {
  isFailed: boolean;
  isFailedCreate: boolean;
  isFailedDelete: boolean;
  isFailedUpdate: boolean;
  isFetching: boolean;
  isFetchingCreate: boolean;
  isFetchingDelete: boolean,
  isFetchingUpdate: boolean,
  list: Array<IGetCategoryResponse> | null;
}

export {
  errorTypes
};

export default useCatalog;
