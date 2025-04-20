import languages from 'constants/languages';
const DEFAULT_LANG = languages.en;

function getMessages(lang: string) {
  const defaultMessages = require('./messages.json');
  let messages: any;
  try {
    messages = lang === DEFAULT_LANG
      ? defaultMessages
      : require(`./messages.${lang.toLowerCase()}.json`);
  } catch (e) {
    messages = defaultMessages;
  }

  return Object
    .entries(defaultMessages)
    .reduce((result, [defaultMessageKey, defaultMessageText]) => ({
      ...result,
      [defaultMessageKey]: messages[defaultMessageKey] || defaultMessageText,
    }), {});
}

export default getMessages;
