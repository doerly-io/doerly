import { useMemo } from 'react';
import {
  useSearchParams,
} from 'react-router-dom';

const useLocationSearch = () => {
  const [ searchParams ] = useSearchParams();

  return useMemo(
    () => Object.fromEntries(new URLSearchParams(searchParams)),
    [searchParams]
  );
};

export default useLocationSearch;
