import React from 'react';
// import PageContainer from 'components/PageContainer';
import DefaultPage from 'pages/default';
// import PageAccessValidator from '../components/PageAccessValidator';
import AuthModes from 'constants/authoritiesValidationModes';
import PageAccessValidator from 'components/PageAccessValidator';
import PageContainer from 'components/PageContainer';

function Default() {
  return (
    <PageAccessValidator mode={AuthModes.ALL}>
      <PageContainer>
        <DefaultPage />
      </PageContainer>
    </PageAccessValidator>
  );
}

export default Default;
