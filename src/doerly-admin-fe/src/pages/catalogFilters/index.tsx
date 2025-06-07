import { useMemo } from 'react';
import {
  IntlProvider as ReactIntlProvider,
} from 'react-intl';
import useLocationSearch from 'hooks/useLocationSearch';
import getMessages from './intl';
import { DEFAULT_LANGUAGE, locales } from 'constants/languages';
import CategoryFilters from './containers/CategoryFilters';

function Index(props: any) {
  const { lang } = useLocationSearch();

  const messages = useMemo(() => getMessages(lang), [lang]);

  return (
    <ReactIntlProvider
      defaultLocale={locales[DEFAULT_LANGUAGE]}
      locale={locales[lang]}
      messages={messages}
    >
      <CategoryFilters {...props} />
    </ReactIntlProvider>
  );
}

export default Index;
