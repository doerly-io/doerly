import axios from 'utils/requests';
import config from 'config';

const { BASE_URL, CATEGORY_SERVICE } = config;


const createCategory = ({
  category,
  onFailed,
  onRequest,
  onSuccess,
}: any) => {
  onRequest();
  return axios.post(`${BASE_URL}${CATEGORY_SERVICE}`, category)
    .then(({ data }) => onSuccess(data))
    .catch(() => onFailed());
};

const deleteCategory = ({
  categoryId,
  onFailed,
  onRequest,
  onSuccess,
}: any) => {
  onRequest();
  return axios.delete(`${BASE_URL}${CATEGORY_SERVICE}/${categoryId}`)
    .then(() => onSuccess())
    .catch(() => onFailed());
};

const getCategories = ({
  onFailed,
  onRequest,
  onSuccess,
}: any) => {
  onRequest();
  return axios.get(`${BASE_URL}${CATEGORY_SERVICE}`)
    .then(({ data }) => onSuccess(data))
    .catch(() => onFailed());
};

const updateCategory = ({
  category,
  categoryId,
  onFailed,
  onRequest,
  onSuccess,
}: any) => {
  onRequest();
  return axios.put(`${BASE_URL}${CATEGORY_SERVICE}/${categoryId}`, category)
    .then(({ data }) => onSuccess(data))
    .catch(() => onFailed());
};

const exportFunctions = {
  createCategory,
  deleteCategory,
  getCategories,
  updateCategory,
};

export default exportFunctions;
