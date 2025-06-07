import axios from 'utils/requests';
import config from 'config';

const {
  BASE_URL,
  BACKEND_URL,
  CATEGORY_SERVICE,
} = config;


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

const getCategoryFilters = ({
  categoryId,
  onFailed,
  onRequest,
  onSuccess,
}: any) => {
  onRequest();
  return axios.get(`${BACKEND_URL}/filter/${categoryId}`)
    .then(({ data }) => onSuccess(data))
    .catch(() => onFailed());
};

const createCategoryFilter = ({
  categoryId,
  filterName,
  filterType,
  onFailed,
  onRequest,
  onSuccess,
}: any) => {
  onRequest();
  const request = {
    categoryId,
    name: filterName,
    type: filterType,
  };
  return axios.post(`${BACKEND_URL}/filter`, request)
    .then(({ data }) => onSuccess(data))
    .catch(() => onFailed());
};

const updateCategoryFilter = ({
  categoryId,
  filterId,
  filterName,
  filterType,
  onFailed,
  onRequest,
  onSuccess,
}: any) => {
  onRequest();
  const request = {
    categoryId,
    name: filterName,
    type: filterType,
  };
  return axios.put(`${BACKEND_URL}/filter/${filterId}`, request)
    .then(({ data }) => onSuccess(data))
    .catch(() => onFailed());
};

const deleteCategoryFilter = ({
  filterId,
  onFailed,
  onRequest,
  onSuccess,
}: any) => {
  onRequest();
  return axios.delete(`${BACKEND_URL}/filter/${filterId}`)
    .then(() => onSuccess())
    .catch(() => onFailed());
};

const getCategoryById = ({
  categoryId,
  onFailed,
  onRequest,
  onSuccess,
}: any) => {
  onRequest();
  return axios.get(`${BASE_URL}${CATEGORY_SERVICE}/${categoryId}`)
    .then(({ data }) => onSuccess(data))
    .catch(() => onFailed());
};

const exportFunctions = {
  createCategory,
  createCategoryFilter,
  deleteCategory,
  deleteCategoryFilter,
  getCategories,
  getCategoryById,
  getCategoryFilters,
  updateCategory,
  updateCategoryFilter,
};

export default exportFunctions;
