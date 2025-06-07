import React from 'react';
import PageContainer from 'components/PageContainer';
import PageAccessValidator from 'components/PageAccessValidator';
import authoritiesValidationModes from 'constants/authoritiesValidationModes';
import authoritiesUI from 'constants/authoritiesUI';
import CatalogFiltersPage from 'pages/catalogFilters';

function CatalogFilters() {
  return (
    <PageContainer>
      <PageAccessValidator
        mode={authoritiesValidationModes.ALL}
        neededAuthorities={authoritiesUI.authorities.CATALOG_MANAGE}
      >
        <CatalogFiltersPage />
      </PageAccessValidator>
    </PageContainer>
  );
}

export default CatalogFilters;
