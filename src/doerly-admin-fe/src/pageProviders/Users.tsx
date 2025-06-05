import authoritiesValidationModes from 'constants/authoritiesValidationModes';
import authoritiesUI from 'constants/authoritiesUI';
import UsersPage from '../pages/users';
import PageContainer from 'components/PageContainer';
import PageAccessValidator from 'components/PageAccessValidator';

function Contractor() {
  return (
    <PageContainer>
      <PageAccessValidator
        mode={authoritiesValidationModes.ALL}
        neededAuthorities={authoritiesUI.authorities.USERS_MANAGE}
      >
        <UsersPage/>
      </PageAccessValidator>
    </PageContainer>
  );
}

export default Contractor;
