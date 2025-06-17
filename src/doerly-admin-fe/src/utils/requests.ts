import axios from 'axios';
import config from 'config';
import storage, { keys } from 'utils/storage';


axios.interceptors.request.use((params) => {
  const token = storage.getItem(keys.ACCESS_TOKEN);
  if (token) {
    params.headers.setAuthorization(`Bearer ${token}`);
  }
  return params;
});

const addAxiosInterceptors = ({
  onSignOut,
}: any) => {
  axios.interceptors.response.use(
    (response) => response,
    (error) => {
      if (error.response.data?.status === 401
        || error.response.data?.status === 403
      ) {
        onSignOut();
      }
      throw error;
    }
  );
};

const getAllErrorsMessages = (errors: any) => {
  if (!errors) return [];

  const messages: string[] = [];

  // Handle case where errors is an object with arrays of messages
  Object.values(errors).forEach(fieldErrors => {
    if (Array.isArray(fieldErrors)) {
      messages.push(...fieldErrors);
    } else if (typeof fieldErrors === 'string') {
      messages.push(fieldErrors);
    }
  });

  return messages;
};

export {
  addAxiosInterceptors,
  getAllErrorsMessages
};

export default axios;
