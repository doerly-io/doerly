import React from 'react';
import { Provider } from 'react-redux';

import { addReduxInterceptors } from 'utils/requests';
import reduxUtils from 'utils/redux';

import App from './containers/App';
import rootReducer from './reducers';

const store = reduxUtils.configureStore(rootReducer);
addReduxInterceptors(store);

export default function Index() {
  return (
    <Provider store={store} >
      <App />
    </Provider>
  );
}
