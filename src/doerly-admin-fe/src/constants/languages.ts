const list = {
  en: 'en',
  ua: 'ua',
};

type TLangKeys = typeof list[keyof typeof list]

export type TLanguages = {
  [K in TLangKeys]: string;
};

export const locales: TLanguages = {
  [list.en]: 'en-US',
  [list.ua]: 'uk-UA',
};

export const DEFAULT_LANGUAGE = list.ua;

export default list;
