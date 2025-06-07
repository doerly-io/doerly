import React, { useMemo } from 'react';
import {
  IntlProvider as ReactIntlProvider,
} from 'react-intl';

import Default from './containers/Statistics';
import useLocationSearch from 'hooks/useLocationSearch';
import getMessages from './intl';

import { DEFAULT_LANGUAGE, locales } from 'constants/languages';

function Index(props: any) {
  const { lang } = useLocationSearch();

  const messages = useMemo(() => getMessages(lang), [lang]);

  return (
    <ReactIntlProvider
      defaultLocale={locales[DEFAULT_LANGUAGE]}
      locale={locales[lang]}
      messages={messages}
    >
      <Default {...props} />
    </ReactIntlProvider>
  );
}

export default Index;
