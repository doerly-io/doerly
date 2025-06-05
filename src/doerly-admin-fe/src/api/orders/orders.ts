import config from 'config';
import axios from 'utils/requests';
import {
  GetOrdersWithPaginationByPredicatesRequest,
} from 'api/orders/model';

const { BASE_URL, ORDERS_SERVICE } = config;

const getOrders = ({
  customerId,
  executorId,
  onFailed,
  onRequest,
  onSuccess,
  pageIndex,
  pageSize,
}: any) => {
  onRequest();

  const requestData: GetOrdersWithPaginationByPredicatesRequest = {
    customerId,
    executorId,
    pageInfo: {
      number: pageIndex,
      size: pageSize,
    },
  };

  return axios.post(`${BASE_URL}${ORDERS_SERVICE}/list`, requestData)
    .then(({ data }) => onSuccess(data))
    .catch(() => onFailed());
};

const exportFunctions = {
  getOrders,
};

export default exportFunctions;
