import axios from 'axios';
import config from 'config';

const {
  BASE_URL,
  PROFILE_SERVICE,
} = config;

const setIsEnabled = ({
  userId,
  isEnabled,
  onFailed,
  onRequest,
  onSuccess,
}: any) => {
  onRequest();
  return axios.post(`${BASE_URL}${PROFILE_SERVICE}/${userId}/is-enabled`, {
    isEnabled,
  })
    .then(() => onSuccess())
    .catch(() => onFailed());
};

const getProfilesInfos = ({
  onFailed,
  onRequest,
  onSuccess,
}: any) => {
  onRequest();
  return axios.get(`${BASE_URL}${PROFILE_SERVICE}`)
    .then(({ data }) => onSuccess(data))
    .catch(() => onFailed());
};

const searchCompleteProfiles = ({
  name,
  number,
  onFailed,
  onRequest,
  onSuccess,
  size,
}: any) => {
  onRequest();
  return axios.post(`${BASE_URL}${PROFILE_SERVICE}/_search`, {
    name,
    number,
    size,
  })
    .then(({ data }) => onSuccess(data))
    .catch(() => onFailed());
};

const apiProfile = {
  getProfilesInfos,
  searchCompleteProfiles,
  setIsEnabled,
};

export default apiProfile;
