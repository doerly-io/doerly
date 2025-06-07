import React from 'react';
import ReportingPage from 'pages/reporting';
import PageContainer from 'components/PageContainer';
import PageAccessValidator from 'components/PageAccessValidator';
import authoritiesValidationModes from 'constants/authoritiesValidationModes';
import authoritiesUI from 'constants/authoritiesUI';

function Reporting() {
  return (
    <PageContainer>
      <PageAccessValidator
        mode={authoritiesValidationModes.ALL}
        neededAuthorities={authoritiesUI.authorities.REPORTING_MANAGE}
      >
        <ReportingPage />
      </PageAccessValidator>
    </PageContainer>
  );
}

export default Reporting; 
