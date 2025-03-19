import React, { useEffect, useState } from 'react';
import {
  BrowserRouter,
  Routes,
  Route,
} from 'react-router-dom';
import PageDefault from 'pageProviders/Default';
import pageURLs from 'constants/pagesURLs';

import * as pages from 'constants/pages';

function App() {
  const [state, setState] = useState({
    componentDidMount: false,
  });

  useEffect(() => {
    setState({
      ...state,
      componentDidMount: true,
    });
  }, []);

  return (
    <BrowserRouter>
      {state.componentDidMount && (
        <Routes>
          <Route
            element={<PageDefault/>}
            path={`${pageURLs[pages.defaultPage]}`}
          />
          <Route
            element={<PageDefault/>}
            path="*"
          />
        </Routes>
      )}
    </BrowserRouter>
  );
}

export default App;
