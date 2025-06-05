import { useEffect, useRef, useState } from 'react';
import { IProfileDto, IProfileSearchResponse } from 'api/profiles/model';
import apiProfile from 'api/profiles/profiles';
import useLocationSearch from 'hooks/useLocationSearch';
import useDebounce from 'hooks/useDebounce';

import * as paginationSizes from 'pages/orders/constants/paginationSizes';
import useChangePage from 'hooks/useChangePage';

const useUsers = () => {
  const componentDidMount = useRef(false);
  const locationSearch = useLocationSearch();
  const pageIndex = +locationSearch.pageIndex || 0;
  const pageSize = +locationSearch.pageSize || paginationSizes.SMALL;
  const changePage = useChangePage();

  const [containerState, setContainerState] = useState({
    searchText: '',
  });
  
  const [profiles, setProfilesState] = useState<IProfilesState>({
    isFailed: false,
    isFetching: false,
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
    handleSearchTextChange,
    isFetching: profiles.isFetching,
    profiles: profiles.list,
  };
};

interface IProfilesState {
  isFailed: boolean;
  isFetching: boolean;
  list: Array<IProfileDto> | null;
  totalCount: number;
  totalPages: number;
}

export default useUsers;
