import { useEffect, useRef, useState } from 'react';
import apiCatalog from 'api/catalog/catalog';
import { IFilter, FilterType, IGetCategoryResponse } from 'api/catalog/model';

const errorTypes = {
  EMPTY_FIELD_NAME: 'EMPTY_FIELD_NAME',
};

const filterFields: { [key: string]: string } = {
  NAME: 'name',
  TYPE: 'type',
};

const fieldsToProperties: Record<string, keyof IFilter> = {
  [filterFields.NAME]: 'name',
  [filterFields.TYPE]: 'type',
};

interface IFiltersState {
  isFailed: boolean;
  isFailedCreate: boolean;
  isFailedDelete: boolean;
  isFailedUpdate: boolean;
  isFetching: boolean;
  isFetchingCreate: boolean;
  isFetchingDelete: boolean;
  isFetchingUpdate: boolean;
  list: IFilter[] | null;
}

interface ICategoryState {
  isFailed: boolean;
  isFetching: boolean;
  data: IGetCategoryResponse | null;
}

const useCategoryFilters = (categoryId: number) => {
  const componentDidMount = useRef(false);

  const [category, setCategoryState] = useState<ICategoryState>({
    data: null,
    isFailed: false,
    isFetching: false,
  });

  const [containerState, setContainerState] = useState({
    editableFilterId: null as number | null,
    filterIdToDelete: null as number | null,
    showAfterCreateFilterSuccessAlert: false,
    showAfterDeleteFilterSuccessAlert: false,
    showAfterUpdateFilterSuccessAlert: false,
    showCreateUpdateDialog: false,
    validationErrors: [] as string[],
  });

  const defaultEditableFilter: IFilter = {
    categoryId,
    id: 0,
    name: '',
    options: [],
    type: FilterType.Checkbox,
  };

  const [editableFilter, setEditableFilter] =
    useState<IFilter>(defaultEditableFilter);

  const [filters, setFiltersState] = useState<IFiltersState>({
    isFailed: false,
    isFailedCreate: false,
    isFailedDelete: false,
    isFailedUpdate: false,
    isFetching: false,
    isFetchingCreate: false,
    isFetchingDelete: false,
    isFetchingUpdate: false,
    list: null,
  });

  const isFetchingSave = filters.isFetchingCreate || filters.isFetchingUpdate;

  const fetchFilters = () => {
    apiCatalog.getCategoryFilters({
      categoryId,
      onFailed: () => setFiltersState((prevState) => ({
        ...prevState,
        isFailed: true,
        isFetching: false,
      })),
      onRequest: () => setFiltersState((prevState) => ({
        ...prevState,
        isFailed: false,
        isFetching: true,
      })),
      onSuccess: ({ value }: any) => setFiltersState((prevState) => ({
        ...prevState,
        isFailed: false,
        isFetching: false,
        list: value as IFilter[],
      })),
    });
  };

  const fetchCategory = () => {
    apiCatalog.getCategoryById({
      categoryId,
      onFailed: () => setCategoryState((prevState) => ({
        ...prevState,
        isFailed: true,
        isFetching: false,
      })),
      onRequest: () => setCategoryState((prevState) => ({
        ...prevState,
        isFailed: false,
        isFetching: true,
      })),
      onSuccess: ({ value }: any) => setCategoryState((prevState) => ({
        ...prevState,
        data: value as IGetCategoryResponse,
        isFailed: false,
        isFetching: false,
      })),
    });
  };

  const getValidationErrors = () => {
    const errors = [];
    if (!editableFilter.name.trim()) {
      errors.push(errorTypes.EMPTY_FIELD_NAME);
    }
    return errors;
  };

  const handleCloseAfterCreateFilterSuccessAlert = () => {
    setContainerState(prevState => ({
      ...prevState,
      showAfterCreateFilterSuccessAlert: false,
    }));
  };

  const handleCloseAfterDeleteFilterSuccessAlert = () => {
    setContainerState(prevState => ({
      ...prevState,
      showAfterDeleteFilterSuccessAlert: false,
    }));
  };

  const handleCloseAfterUpdateFilterSuccessAlert = () => {
    setContainerState(prevState => ({
      ...prevState,
      showAfterUpdateFilterSuccessAlert: false,
    }));
  };

  const handleCloseCreateUpdateDialog = () => {
    setContainerState(prevState => ({
      ...prevState,
      showCreateUpdateDialog: false,
    }));
  };

  const handleCloseDeleteDialog = () => {
    setContainerState(prevState => ({
      ...prevState,
      filterIdToDelete: null,
    }));
  };

  const handleConfirmDeleteFilter = () => {
    apiCatalog.deleteCategoryFilter({
      filterId: containerState.filterIdToDelete as number,
      onFailed: () => setFiltersState((prevState) => ({
        ...prevState,
        isFailedDelete: true,
        isFetchingDelete: false,
      })),
      onRequest: () => setFiltersState((prevState) => ({
        ...prevState,
        isFailedDelete: false,
        isFetchingDelete: true,
      })),
      onSuccess: () => setFiltersState((prevState) => ({
        ...prevState,
        isFetchingDelete: false,
      })),
    });
  };

  const handleConfirmSaveFilter = () => {
    const validationErrors = getValidationErrors();
    if (!validationErrors.length) {
      if (containerState.editableFilterId) {
        apiCatalog.updateCategoryFilter({
          categoryId,
          filterId: containerState.editableFilterId,
          filterName: editableFilter.name,
          filterType: editableFilter.type,
          onFailed: () => setFiltersState((prevState) => ({
            ...prevState,
            isFailedUpdate: true,
            isFetchingUpdate: false,
          })),
          onRequest: () => setFiltersState((prevState) => ({
            ...prevState,
            isFailedUpdate: false,
            isFetchingUpdate: true,
          })),
          onSuccess: () => setFiltersState((prevState) => ({
            ...prevState,
            isFetchingUpdate: false,
          })),
        });
      } else {
        apiCatalog.createCategoryFilter({
          categoryId,
          filterName: editableFilter.name,
          filterType: editableFilter.type,
          onFailed: () => setFiltersState((prevState) => ({
            ...prevState,
            isFailedCreate: true,
            isFetchingCreate: false,
          })),
          onRequest: () => setFiltersState((prevState) => ({
            ...prevState,
            isFailedCreate: false,
            isFetchingCreate: true,
          })),
          onSuccess: () => setFiltersState((prevState) => ({
            ...prevState,
            isFetchingCreate: false,
          })),
        });
      }
    }
    setContainerState(prevState => ({
      ...prevState,
      validationErrors,
    }));
  };

  const handleEditableFieldChange = ({
    fieldKey,
    value,
  }: any) => {
    setEditableFilter(prevState => ({
      ...prevState,
      [fieldsToProperties[fieldKey]]: value,
    }));
  };

  const handleStartCreate = () => {
    setEditableFilter({
      ...defaultEditableFilter,
      categoryId,
    });
    setContainerState(prevState => ({
      ...prevState,
      showCreateUpdateDialog: true,
    }));
  };

  const handleStartDelete = (filterId: number) => {
    setContainerState(prevState => ({
      ...prevState,
      filterIdToDelete: filterId,
    }));
  };

  const handleStartEdit = (filter: IFilter) => {
    setEditableFilter(filter);
    setContainerState(prevState => ({
      ...prevState,
      editableFilterId: filter.id,
      showCreateUpdateDialog: true,
    }));
  };

  useEffect(() => {
    if (componentDidMount.current
      && !filters.isFetchingDelete
      && !filters.isFailedDelete
    ) {
      setContainerState(prevState => ({
        ...prevState,
        filterIdToDelete: null,
        showAfterDeleteFilterSuccessAlert: true,
      }));
    }
  }, [filters.isFetchingDelete]);

  useEffect(() => {
    if (componentDidMount.current
      && !filters.isFetchingCreate
      && !filters.isFailedCreate
    ) {
      setContainerState(prevState => ({
        ...prevState,
        showAfterCreateFilterSuccessAlert: true,
        showCreateUpdateDialog: false,
      }));
      setEditableFilter(defaultEditableFilter);
    }
  }, [filters.isFetchingCreate]);

  useEffect(() => {
    if (componentDidMount.current
      && !filters.isFetchingUpdate
      && !filters.isFailedUpdate
    ) {
      setContainerState(prevState => ({
        ...prevState,
        editableFilterId: null,
        showAfterUpdateFilterSuccessAlert: true,
        showCreateUpdateDialog: false,
      }));
      setEditableFilter(defaultEditableFilter);
    }
  }, [filters.isFetchingUpdate]);

  useEffect(() => {
    if (componentDidMount.current
      && !filters.isFailedCreate
      && !filters.isFailedDelete
      && !filters.isFailedUpdate
      && !filters.isFetchingCreate
      && !filters.isFetchingDelete
      && !filters.isFetchingUpdate
    ) {
      fetchFilters();
    }
  }, [
    filters.isFetchingCreate,
    filters.isFetchingDelete,
    filters.isFetchingUpdate,
  ]);

  useEffect(() => {
    if (!componentDidMount.current) {
      componentDidMount.current = true;
      fetchCategory();
      fetchFilters();
    }
  }, []);

  return {
    category,
    containerState,
    editableFilter,
    filterFields,
    filters: filters.list,
    handleCloseAfterCreateFilterSuccessAlert,
    handleCloseAfterDeleteFilterSuccessAlert,
    handleCloseAfterUpdateFilterSuccessAlert,
    handleCloseCreateUpdateDialog,
    handleCloseDeleteDialog,
    handleConfirmDeleteFilter,
    handleConfirmSaveFilter,
    handleEditableFieldChange,
    handleStartCreate,
    handleStartDelete,
    handleStartEdit,
    isFailed: filters.isFailed,
    isFetching: filters.isFetching,
    isFetchingDelete: filters.isFetchingDelete,
    isFetchingSave,
    showAfterCreateFilterSuccessAlert:
      containerState.showAfterCreateFilterSuccessAlert,
    showAfterDeleteFilterSuccessAlert:
      containerState.showAfterDeleteFilterSuccessAlert,
    showAfterUpdateFilterSuccessAlert:
      containerState.showAfterUpdateFilterSuccessAlert,
  };
};

export {
  errorTypes,
  FilterType,
  useCategoryFilters
};
