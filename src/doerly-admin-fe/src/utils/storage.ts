const getItem = (key: string): any => {
  return localStorage.getItem(key);
};

const removeItem = (key: string) => {
  localStorage.removeItem(key);
};

const setItem = (key: string, value: any) => {
  localStorage.setItem(key, value);
};

const forExport = {
  getItem,
  removeItem,
  setItem,
};

export default forExport;
