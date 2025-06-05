import React from 'react';
import SettingsPage from 'pages/settings';
import PageContainer from 'components/PageContainer';
import PageAccessValidator from 'components/PageAccessValidator';
import authoritiesValidationModes from 'constants/authoritiesValidationModes';
import authoritiesUI from 'constants/authoritiesUI';

function Settings() {
  return (
    <PageContainer>
      <PageAccessValidator
        mode={authoritiesValidationModes.ALL}
        neededAuthorities={authoritiesUI.authorities.SETTINGS_MANAGE}
      >
        <SettingsPage />
      </PageAccessValidator>
    </PageContainer>
  );
}

export default Settings;
