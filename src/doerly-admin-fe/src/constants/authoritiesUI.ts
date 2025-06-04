const authorities = {
  CATALOG_MANAGE: 'CATALOG_MANAGE',
  ORDERS_MANAGE: 'ORDERS_MANAGE',
  REPORTING_MANAGE: 'REPORTING_MANAGE',
  SETTINGS_MANAGE: 'SETTINGS_MANAGE',
  USERS_MANAGE: 'USERS_MANAGE',
};

const roles = {
  ADMIN: 'ADMIN',
};

const roleAuthorities = {
  [roles.ADMIN]: [
    authorities.CATALOG_MANAGE,
    authorities.ORDERS_MANAGE,
    authorities.REPORTING_MANAGE,
    authorities.SETTINGS_MANAGE,
    authorities.USERS_MANAGE,
  ],
};

const toExport = {
  authorities,
  roleAuthorities,
  roles,
};

export default toExport;
