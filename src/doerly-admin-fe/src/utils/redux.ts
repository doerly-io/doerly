import {
  applyMiddleware,
  createStore,
  AnyAction,
  PreloadedState,
  Reducer,
  Store,
} from 'redux';
import thunkMiddleware from 'redux-thunk';

const createStoreWithMiddleware = applyMiddleware(thunkMiddleware)(createStore);

function configureStore<S, A extends AnyAction>(
  rootReducer: Reducer<S, A>,
  initialState?: PreloadedState<S>
): Store<S, A> {
  return createStoreWithMiddleware(rootReducer, initialState);
}

const forExport = {
  configureStore,
};

export default forExport;
