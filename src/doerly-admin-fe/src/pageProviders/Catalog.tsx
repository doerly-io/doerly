import CatalogPage from 'pages/catalog';
import PageContainer from 'components/PageContainer';
import PageAccessValidator from 'components/PageAccessValidator';
import authoritiesValidationModes from 'constants/authoritiesValidationModes';
import authoritiesUI from 'constants/authoritiesUI';

function Catalog() {
  return (
    <PageContainer>
      <PageAccessValidator
        mode={authoritiesValidationModes.ALL}
        neededAuthorities={authoritiesUI.authorities.CATALOG_MANAGE}
      >
        <CatalogPage />
      </PageAccessValidator>
    </PageContainer>
  );
}

export default Catalog; 
