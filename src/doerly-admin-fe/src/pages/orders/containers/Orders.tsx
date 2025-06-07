import { useIntl } from 'react-intl';
import { makeStyles } from 'tss-react/mui';
import Accordion from 'components/Accordion';
import AccordionDetails from 'components/AccordionDetails';
import AccordionSummary from 'components/AccordionSummary';
import Breadcrumb from 'components/Breadcrumb';
import Breadcrumbs from 'components/Breadcrumbs';
import Card from 'components/Card';
import CardContent from 'components/CardContent';
import Colon from 'components/Colon';
import Divider from 'components/Divider';
import Typography from 'components/Typography';
import useIsMobile, { useIsSmallMobile } from 'hooks/useIsMobile';
import useChangePage from 'hooks/useChangePage';
import useLocationSearch from 'hooks/useLocationSearch';
import Select from 'components/Select';
import useTheme from 'hooks/useTheme';
import MenuItem from 'components/MenuItem';
import useOrders, {
  ALL,
  OrderStatusesColors,
} from '../hooks/useOrders';
import IconChevronDown from 'components/icons/ChevronDown';
import { IProfileInfo } from 'api/profiles/model';
import Show from 'components/Show';
import Loading from 'components/Loading';
import Chip from 'components/Chip';
import getInitials from 'utils/stringHelper';

const getClasses = makeStyles<any>()((_, theme: any) => ({
  actionElement: {
    minWidth: '165px',
  },
  actionsContainer: {
    alignItems: 'center',
    display: 'flex',
    gap: `${theme.spacing(2)}px`,
  },
  actionsInnerContainer: {
    alignItems: 'center',
    display: 'flex',
    gap: `${theme.spacing(2)}px`,
  },
  avatar: {
    borderRadius: '50%',
    height: '40px',
    objectFit: 'cover',
    width: '40px',
  },
  avatarPlaceholder: {
    alignItems: 'center',
    backgroundColor: theme.colors.cobalt,
    borderRadius: '50%',
    color: theme.colors.black,
    display: 'flex',
    fontSize: '16px',
    fontWeight: 'bold',
    height: '40px',
    justifyContent: 'center',
    width: '40px',
  },
  container: {
    display: 'flex',
    flexDirection: 'column',
    gap: `${theme.spacing(2)}px`,
  },
  customerContainer: {
    alignItems: 'center',
    display: 'flex',
    gap: `${theme.spacing(1)}px`,
  },
  detailsContainer: {
    display: 'flex',
    flexDirection: 'column',
    gap: `${theme.spacing(1)}px`,
    overflowY: 'hidden',
  },
  detailsLabel: {
    alignItems: 'center',
    display: 'flex',
    flex: 1,
    gap: `${theme.spacing(1)}px`,
  },
  detailsRow: {
    alignItems: 'center',
    display: 'flex',
    gap: `${theme.spacing(2)}px`,
    height: '32px',
  },
  detailsRowMobile: {
    display: 'flex',
    flexDirection: 'column',
    gap: `${theme.spacing(1)}px`,
  },
  detailsValue: {
    alignItems: 'center',
    display: 'flex',
    flex: 2,
    gap: `${theme.spacing(1)}px`,
    width: '50%',
  },
  detailsValueValueMobile: {
    display: 'flex',
    flexDirection: 'column',
    gap: `${theme.spacing(2)}px`,
    width: '100%',
  },
  nameContainer: {
    display: 'flex',
    flexDirection: 'column',
  },
  summaryCell: {
    flex: 1,
    minWidth: '50px',
  },
  summaryContainer: {
    alignItems: 'center',
    display: 'flex',
    gap: `${theme.spacing(2)}px`,
    justifyContent: 'space-between',
    width: '100%',
  },
  tableCell: {
    maxWidth: 'fit-content',
  },
}));

const ICON_SIZE = 24;

function Orders() {
  const { theme } = useTheme();
  const { classes } = getClasses(theme);
  const { formatMessage } = useIntl();
  const isMobile = useIsMobile();
  const isSmallMobile = useIsSmallMobile();
  const changePage = useChangePage();
  const locationSearch = useLocationSearch();

  const {
    customerId,
    executorId,
    getExecutorDisplayName,
    isFetching,
    orders,
    profileInfos,
  } = useOrders();

  return (
    <div className={classes.container}>
      <Breadcrumbs>
        <Breadcrumb
          label={formatMessage({ id: 'orders' })}
          variant="text"
        />
      </Breadcrumbs>
      <div className={classes.actionsContainer}>
        <div className={classes.actionElement}>
          <Select
            fullWidth
            label={formatMessage({
              id: 'label.filter.customer',
            })}
            onChange={({ target }) => {
              changePage({
                locationSearch: {
                  ...locationSearch,
                  customerId: target.value,
                  pageIndex: 0,
                },
                replace: true,
              });
            }}
            size="small"
            value={customerId || ALL}
            variant="outlined"
          >
            <MenuItem value={ALL}>
              <Typography>
                {formatMessage({
                  id: 'all',
                })}
              </Typography>
            </MenuItem>
            {profileInfos.map((info: IProfileInfo) => (
              <MenuItem
                key={info.id.toString()}
                value={info.id.toString()}
              >
                <Typography noWrap>
                  {info.firstName} {info.lastName}
                </Typography>
              </MenuItem>
            ))}
          </Select>
        </div>
        <div className={classes.actionElement}>
          <Select
            fullWidth
            label={formatMessage({
              id: 'label.filter.executor',
            })}
            onChange={({ target }) => {
              changePage({
                locationSearch: {
                  ...locationSearch,
                  executorId: target.value,
                  pageIndex: 0,
                },
                replace: true,
              });
            }}
            size="small"
            value={executorId || ALL}
            variant="outlined"
          >
            <MenuItem value={ALL}>
              <Typography>
                {formatMessage({
                  id: 'all',
                })}
              </Typography>
            </MenuItem>
            {profileInfos.map((info: IProfileInfo) => (
              <MenuItem
                key={info.id.toString()}
                value={info.id.toString()}
              >
                <Typography noWrap>
                  {info.firstName} {info.lastName}
                </Typography>
              </MenuItem>
            ))}
          </Select>
        </div>
      </div>
      <Show condition={isFetching}>
        <Loading variant='loading' />
      </Show>
      <Show condition={!isFetching}>
        <Show condition={!orders?.length}>
          <Loading variant="noData">
            <Typography
              align="center"
              color="secondary"
              variant="subtitle"
            >
              {formatMessage({ id: 'loading.noData' })}
            </Typography>
          </Loading>
        </Show>
        <div>
          {orders?.map((order) => (
            <>
              <Accordion key={order.id}>
                <AccordionSummary expandIcon={
                  !isSmallMobile
                    ? <IconChevronDown size={ICON_SIZE}/>
                    : undefined
                }>
                  <div className={classes.summaryContainer}>
                    <div className={classes.summaryCell}>
                      <div className={classes.customerContainer}>
                        <Show condition={!isSmallMobile}>
                          {order.customer.avatarUrl ? (
                            <img
                              src={order.customer.avatarUrl}
                              /* eslint-disable-next-line max-len */
                              alt={`${order.customer.firstName} ${order.customer.lastName}`}
                              className={classes.avatar}
                            />
                          ) : (
                            <div className={classes.avatarPlaceholder}>
                              {getInitials(order.customer.firstName,
                                order.customer.lastName)}
                            </div>
                          )}
                        </Show>
                        <div className={classes.nameContainer}>
                          <Typography noWrap>
                            {order.customer.firstName}
                          </Typography>
                          <Typography noWrap>
                            {order.customer.lastName}
                          </Typography>
                        </div>
                      </div>
                    </div>
                    <div className={classes.summaryCell}>
                      <Typography noWrap>
                        {order.name}
                      </Typography>
                    </div>
                    <Show condition={!isMobile}>
                      <div className={classes.summaryCell}>
                        <Chip
                          color={OrderStatusesColors[order.status]}
                          label={formatMessage({
                            id: `label.order.status.${order.status}`,
                          })}
                        />
                      </div>
                    </Show>
                  </div>
                </AccordionSummary>
                <Divider color={theme.colors.cobalt}/>
                <AccordionDetails disablePadding>
                  <Card>
                    <CardContent>
                      <div className={classes.detailsContainer}>
                        {/* name */}
                        <div className={isMobile
                          ? classes.detailsRowMobile
                          : classes.detailsRow
                        }>
                          <div className={classes.detailsLabel}>
                            <Typography color="secondary">
                              <Colon>
                                {formatMessage({ id: 'label.order.name' })}
                              </Colon>
                            </Typography>
                          </div>
                          <div className={isMobile
                            ? classes.detailsValueValueMobile
                            : classes.detailsValue
                          }>
                            <Typography wordBreak="break-word">
                              {order.name}
                            </Typography>
                          </div>
                        </div>
                        {/* description */}
                        <div className={isMobile
                          ? classes.detailsRowMobile
                          : classes.detailsRow
                        }>
                          <div className={classes.detailsLabel}>
                            <Typography color="secondary">
                              <Colon>
                                {formatMessage({
                                  id: 'label.order.description',
                                })}
                              </Colon>
                            </Typography>
                          </div>
                          <div className={isMobile
                            ? classes.detailsValueValueMobile
                            : classes.detailsValue
                          }>
                            <Typography wordBreak="break-word">
                              {order.description}
                            </Typography>
                          </div>
                        </div>
                        {/* price */}
                        <div className={isMobile
                          ? classes.detailsRowMobile
                          : classes.detailsRow
                        }>
                          <div className={classes.detailsLabel}>
                            <Typography color="secondary">
                              <Colon>
                                {formatMessage({ id: 'label.order.price' })}
                              </Colon>
                            </Typography>
                          </div>
                          <div className={isMobile
                            ? classes.detailsValueValueMobile
                            : classes.detailsValue
                          }>
                            <Typography>
                              {`${order.price} ${formatMessage({
                                id: 'currency',
                              })}`}
                            </Typography>
                          </div>
                        </div>
                        {/*is price negotiable*/}
                        <div className={isMobile
                          ? classes.detailsRowMobile
                          : classes.detailsRow
                        }>
                          <div className={classes.detailsLabel}>
                            <Typography color="secondary">
                              <Colon>
                                {formatMessage({
                                  id: 'label.order.price.negotiable',
                                })}
                              </Colon>
                            </Typography>
                          </div>
                          <div className={isMobile
                            ? classes.detailsValueValueMobile
                            : classes.detailsValue
                          }>
                            <Typography>
                              {formatMessage({
                                id: order.isPriceNegotiable
                                  ? 'label.yes'
                                  : 'label.no',
                              })}
                            </Typography>
                          </div>
                        </div>
                        {/* payment kind */}
                        <div className={isMobile
                          ? classes.detailsRowMobile
                          : classes.detailsRow
                        }>
                          <div className={classes.detailsLabel}>
                            <Typography color="secondary">
                              <Colon>
                                {formatMessage({
                                  id: 'label.order.paymentKind',
                                })}
                              </Colon>
                            </Typography>
                          </div>
                          <div className={isMobile
                            ? classes.detailsValueValueMobile
                            : classes.detailsValue
                          }>
                            <Typography>
                              {formatMessage({
                                // eslint-disable-next-line max-len
                                id: `label.order.paymentKind.${order.paymentKind.toString()}`,
                              })}
                            </Typography>
                          </div>
                        </div>
                        {/* due date */}
                        <div className={isMobile
                          ? classes.detailsRowMobile
                          : classes.detailsRow
                        }>
                          <div className={classes.detailsLabel}>
                            <Typography color="secondary">
                              <Colon>
                                {formatMessage({ id: 'label.order.dueDate' })}
                              </Colon>
                            </Typography>
                          </div>
                          <div className={isMobile
                            ? classes.detailsValueValueMobile
                            : classes.detailsValue
                          }>
                            <Typography>
                              {new Date(order.dueDate).toLocaleDateString()}
                            </Typography>
                          </div>
                        </div>
                        {/* status */}
                        <div className={isMobile
                          ? classes.detailsRowMobile
                          : classes.detailsRow
                        }>
                          <div className={classes.detailsLabel}>
                            <Typography color="secondary">
                              <Colon>
                                {formatMessage({ id: 'label.order.status' })}
                              </Colon>
                            </Typography>
                          </div>
                          <div className={classes.detailsValue}>
                            <Chip
                              color={OrderStatusesColors[order.status]}
                              label={formatMessage({
                                id: `label.order.status.${order.status}`,
                              })}
                            />
                          </div>
                        </div>
                        {/*customer*/}
                        <div className={isMobile
                          ? classes.detailsRowMobile
                          : classes.detailsRow
                        }>
                          <div className={classes.detailsLabel}>
                            <Typography color="secondary">
                              <Colon>
                                {formatMessage({ id: 'label.order.customer' })}
                              </Colon>
                            </Typography>
                          </div>
                          <div className={isMobile
                            ? classes.detailsValueValueMobile
                            : classes.detailsValue
                          }>
                            <Typography
                              noWrap
                              wordBreak="break-word"
                            >
                              {/*eslint-disable-next-line max-len*/}
                              {order.customer.firstName} {order.customer.lastName}
                            </Typography>
                          </div>
                        </div>
                        {/* customer completion confirmed */}
                        <div className={isMobile
                          ? classes.detailsRowMobile
                          : classes.detailsRow
                        }>
                          <div className={classes.detailsLabel}>
                            <Typography color="secondary">
                              <Colon>
                                {formatMessage({
                                  id: 'label.order.customerCompletionConfirmed',
                                })}
                              </Colon>
                            </Typography>
                          </div>
                          <div className={isMobile
                            ? classes.detailsValueValueMobile
                            : classes.detailsValue
                          }>
                            <Typography>
                              {formatMessage({
                                id: order.customerCompletionConfirmed
                                  ? 'label.order.completion.confirmed'
                                  : 'label.order.completion.notConfirmed',
                              })}
                            </Typography>
                          </div>
                        </div>
                        {/* executor */}
                        <div className={isMobile
                          ? classes.detailsRowMobile
                          : classes.detailsRow
                        }>
                          <div className={classes.detailsLabel}>
                            <Typography color="secondary">
                              <Colon>
                                {formatMessage({ id: 'label.order.executor' })}
                              </Colon>
                            </Typography>
                          </div>
                          <div className={isMobile
                            ? classes.detailsValueValueMobile
                            : classes.detailsValue
                          }>
                            <Typography>
                              {getExecutorDisplayName(order.executorId)}
                            </Typography>
                          </div>
                        </div>
                        {/*executor completion confirmed*/}
                        <div className={isMobile
                          ? classes.detailsRowMobile
                          : classes.detailsRow
                        }>
                          <div className={classes.detailsLabel}>
                            <Typography color="secondary">
                              <Colon>
                                {formatMessage({
                                  id: 'label.order.executorCompletionConfirmed',
                                })}
                              </Colon>
                            </Typography>
                          </div>
                          <div className={isMobile
                            ? classes.detailsValueValueMobile
                            : classes.detailsValue
                          }>
                            <Typography>
                              {formatMessage({
                                id: order.executorCompletionConfirmed
                                  ? 'label.order.completion.confirmed'
                                  : 'label.order.completion.notConfirmed',
                              })}
                            </Typography>
                          </div>
                        </div>
                        {/* execution date */}
                        <div className={isMobile
                          ? classes.detailsRowMobile
                          : classes.detailsRow
                        }>
                          <div className={classes.detailsLabel}>
                            <Typography color="secondary">
                              <Colon>
                                {formatMessage({
                                  id: 'label.order.executionDate',
                                })}
                              </Colon>
                            </Typography>
                          </div>
                          <div className={isMobile
                            ? classes.detailsValueValueMobile
                            : classes.detailsValue
                          }>
                            <Typography>
                              {order.executionDate
                                ? new Date(order.executionDate)
                                  .toLocaleDateString()
                                : '-'}
                            </Typography>
                          </div>
                        </div>
                      </div>
                    </CardContent>
                  </Card>
                </AccordionDetails>
              </Accordion>
              <Divider color={theme.colors.cobalt}/>
            </>
          ))}
        </div>
      </Show>
    </div>
  );
}

export default Orders;
