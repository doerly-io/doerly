import React, { useMemo } from 'react';
import {
  IntlProvider as ReactIntlProvider,
} from 'react-intl';

import useLocationSearch from 'hooks/useLocationSearch';
import getMessages from 'intl';

import { DEFAULT_LANGUAGE, locales } from 'constants/languages';

function IntlProvider({
  children,
}: IProps) {
  const {
    lang,
  } = useLocationSearch();

  const messages = useMemo(() => getMessages(lang), [lang]);

  return (
    <ReactIntlProvider
      defaultLocale={locales[DEFAULT_LANGUAGE]}
      locale={locales[lang]}
      messages={messages}
    >
      {children}
    </ReactIntlProvider>
  );
}

interface IProps {
  children: React.ReactNode,
}

export default IntlProvider;
