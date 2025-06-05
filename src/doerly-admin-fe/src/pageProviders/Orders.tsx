import React from 'react';
import OrdersPage from 'pages/orders';
import PageContainer from 'components/PageContainer';
import PageAccessValidator from 'components/PageAccessValidator';
import authoritiesValidationModes from 'constants/authoritiesValidationModes';
import authoritiesUI from 'constants/authoritiesUI';

function Orders() {
  return (
    <PageContainer>
      <PageAccessValidator
        mode={authoritiesValidationModes.ALL}
        neededAuthorities={authoritiesUI.authorities.ORDERS_MANAGE}
      >
        <OrdersPage />
      </PageAccessValidator>
    </PageContainer>
  );
}

export default Orders; 
