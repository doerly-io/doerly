import axios from 'axios';
import config from 'config';

const {
  BASE_URL,
  STATISTICS_SERVICE,
} = config;

const getUsersActivity = ({
  onFailed,
  onRequest,
  onSuccess,
}: any) => {
  onRequest();
  return axios.get(`${BASE_URL}${STATISTICS_SERVICE}/activity-users`)
    .then(({ data }) => onSuccess(data))
    .catch(() => onFailed());
};

const getPaymentStatistics = ({
  onFailed,
  onRequest,
  onSuccess,
}: any) => {
  onRequest();
  return axios.get(`${BASE_URL}${STATISTICS_SERVICE}/payment`)
    .then(({ data }) => onSuccess(data))
    .catch(() => onFailed());
};

const exportFunctions = {
  getPaymentStatistics,
  getUsersActivity,
};

export default exportFunctions;
