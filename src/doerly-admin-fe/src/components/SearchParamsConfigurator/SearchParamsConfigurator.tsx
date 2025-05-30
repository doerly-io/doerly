import { useEffect } from 'react';
import { useSearchParams } from 'react-router-dom';
import languages, { DEFAULT_LANGUAGE } from 'constants/languages';

enum LocationSearch {
  Lang = 'lang',
}

// Alternatively, we can have mappings between pages and their
// corresponding default search params
export const DEFAULT_LOCATION_SEARCH: any = {
  [LocationSearch.Lang]: DEFAULT_LANGUAGE,
};

function SearchParamsConfigurator() {
  const [searchParams, setSearchParams] = useSearchParams();

  useEffect(() => {
    let isSearchParamsUpdated = false;
    if (!searchParams.has(LocationSearch.Lang)
      || !Object.values(languages)
        .includes(searchParams.get(LocationSearch.Lang) || '')
    ) {
      searchParams.set(LocationSearch.Lang, DEFAULT_LANGUAGE);
      isSearchParamsUpdated = true;
    }
    if (isSearchParamsUpdated) {
      setSearchParams(searchParams, { replace: true });
    }
  }, [searchParams]);
  return null;
}

export default SearchParamsConfigurator;
