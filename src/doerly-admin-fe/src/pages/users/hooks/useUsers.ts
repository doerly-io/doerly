import { useEffect, useRef, useState } from 'react';
import { IProfileDto, IProfileSearchResponse } from 'api/profiles/model';
import apiProfile from 'api/profiles/profiles';
import useLocationSearch from 'hooks/useLocationSearch';
import useDebounce from 'hooks/useDebounce';

import * as paginationSizes from 'pages/orders/constants/paginationSizes';
import useChangePage from 'hooks/useChangePage';

const toggleItemInArray = (
  array: number[],
  itemId: number
): number[] => {
  const isItemInArray = array.includes(itemId);
  return isItemInArray
    ? array.filter(id => id !== itemId)
    : array.concat(itemId);
};

const useUsers = () => {
  const componentDidMount = useRef(false);
  const locationSearch = useLocationSearch();
  const pageIndex = +locationSearch.pageIndex || 0;
  const pageSize = +locationSearch.pageSize || 1000;
  const changePage = useChangePage();

  const [containerState, setContainerState] = useState({
    expandedAccordionIds: [] as number[],
    searchText: '',
    showAfterUpdateUserSuccessAlert: false,
  });
  
  const [profiles, setProfilesState] = useState<IProfilesState>({
    isFailed: false,
    isFailedUpdate: false,
    isFetching: false,
    isFetchingUpdate: false,
    list: [] as Array<IProfileDto> | null,
    totalCount: 0,
    totalPages: 0,
  });

  const debouncedSearchText = useDebounce({ value: containerState.searchText });
  
  const fetchProfiles = ({
    pageIndex,
    pageSize,
    searchText,
  }: any) => {
    apiProfile.searchCompleteProfiles({
      name: searchText,
      number: pageIndex + 1,
      onFailed: () => setProfilesState((prevState) => ({
        ...prevState,
        isFailed: true,
        isFetching: false,
      })),
      onRequest: () => setProfilesState((prevState) => ({
        ...prevState,
        isFailed: false,
        isFetching: true,
      })),
      onSuccess: ({ value }: any) => {
        const typedResponse = value as IProfileSearchResponse;
        const {
          list: profilesList,
          totalSize,
          pagesCount,
        } = typedResponse;

        setProfilesState((prevState) => ({
          ...prevState,
          isFailed: false,
          isFetching: false,
          list: profilesList,
          pagesCount,
          totalCount: totalSize,
        }));
      },
      size: pageSize,
    });
  };

  const handleAccordionChange = (id: number) => {
    const expandedAccordionIds = toggleItemInArray(
      containerState.expandedAccordionIds, id
    );
    setContainerState({
      ...containerState,
      expandedAccordionIds,
    });
  };

  const handleSearchTextChange = (searchText: string) => {
    if (pageIndex !== 0) {
      changePage({
        locationSearch: {
          ...locationSearch,
          pageIndex: 0,
        },
        replace: true,
      });
    }
    setContainerState((prev) => ({
      ...prev,
      searchText: searchText,
    }));
  };

  const handleToggleEnabled = (userId: number, isEnabled: boolean) => {
    apiProfile.setIsEnabled({
      isEnabled,
      onFailed: () => setProfilesState((prevState) => ({
        ...prevState,
        isFailedUpdate: true,
        isFetchingUpdate: false,
      })),
      onRequest: () => setProfilesState((prevState) => ({
        ...prevState,
        isFailedUpdate: false,
        isFetchingUpdate: true,
      })),
      onSuccess: () => setProfilesState((prevState) => ({
        ...prevState,
        isFetchingUpdate: false,
      })),
      userId,
    });
  };

  const handleCloseAfterUpdateUserSuccessAlert = () => {
    setContainerState(prevState => ({
      ...prevState,
      showAfterUpdateUserSuccessAlert: false,
    }));
  };

  useEffect(() => {
    if (componentDidMount.current
      && !profiles.isFetchingUpdate
      && !profiles.isFailedUpdate
    ) {
      setContainerState(prevState => ({
        ...prevState,
        showAfterUpdateUserSuccessAlert: true,
      }));
    }
  }, [profiles.isFetchingUpdate]);

  useEffect(() => {
    if (componentDidMount.current
      && !profiles.isFailedUpdate
      && !profiles.isFetchingUpdate
    ) {
      fetchProfiles({
        pageIndex,
        pageSize,
        searchText: debouncedSearchText,
      });
    }
  }, [profiles.isFetchingUpdate]);

  useEffect(() => {
    fetchProfiles({
      pageIndex,
      pageSize,
      searchText: debouncedSearchText,
    });
  }, [
    debouncedSearchText,
    pageIndex,
    pageSize,
  ]);

  useEffect(() => {
    componentDidMount.current = true;
  }, []);

  return {
    containerState,
    debouncedSearchText,
    handleAccordionChange,
    handleCloseAfterUpdateUserSuccessAlert,
    handleSearchTextChange,
    handleToggleEnabled,
    isFetching: profiles.isFetching,
    isFetchingUpdate: profiles.isFetchingUpdate,
    profiles: profiles.list,
    showAfterUpdateUserSuccessAlert:
    containerState.showAfterUpdateUserSuccessAlert,
  };
};

interface IProfilesState {
  isFailed: boolean;
  isFetching: boolean;
  isFetchingUpdate: boolean;
  isFailedUpdate: boolean;
  list: Array<IProfileDto> | null;
  totalCount: number;
  totalPages: number;
}

export default useUsers;
