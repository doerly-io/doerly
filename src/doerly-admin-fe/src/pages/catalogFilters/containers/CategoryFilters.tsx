import { useIntl } from 'react-intl';
import { makeStyles } from 'tss-react/mui';
import useTheme from 'hooks/useTheme';
import useIsMobile, { useIsSmallMobile } from 'hooks/useIsMobile';
import {
  useCategoryFilters,
  errorTypes,
  FilterType,
} from '../hooks/useCategoryFilters';
import Typography from 'components/Typography';
import Breadcrumbs from 'components/Breadcrumbs';
import Breadcrumb from 'components/Breadcrumb';
import Loading from 'components/Loading';
import Show from 'components/Show';
import Card from 'components/Card';
import CardTitle from 'components/CardTitle';
import CardContent from 'components/CardContent';
import CardActions from 'components/CardActions';
import Button from 'components/Button';
import IconButton from 'components/IconButton';
import IconClose from 'components/icons/Close';
import IconEdit from 'components/icons/Edit';
import IconDelete from 'components/icons/Delete';
import Dialog from 'components/Dialog';
import TextField from 'components/TextField';
import Colon from 'components/Colon';
import Tooltip from 'components/Tooltip';
import IconError from 'components/icons/Error';
import Snackbar from 'components/Snackbar';
import Select from 'components/Select';
import MenuItem from 'components/MenuItem';
import { useParams } from 'react-router-dom';
import { IFilter } from 'api/catalog/model';
import pagesURLs from 'constants/pagesURLs';
import * as pages from 'constants/pages';

const ICON_SIZE = 20;

const getClasses = makeStyles<any>()((_, theme: any) => ({
  actionsContainer: {
    alignItems: 'center',
    display: 'flex',
    gap: `${theme.spacing(3)}px`,
    justifyContent: 'flex-end',
  },
  container: {
    display: 'flex',
    flexDirection: 'column',
    gap: `${theme.spacing(2)}px`,
  },
  dialogContainer: {
    display: 'flex',
    flexDirection: 'column',
    gap: `${theme.spacing(2.5)}px`,
    overflowY: 'hidden',
  },
  dialogLabel: {
    alignItems: 'center',
    display: 'flex',
    flex: 1,
    gap: `${theme.spacing(1)}px`,
  },
  dialogRow: {
    display: 'flex',
    flexDirection: 'column',
    gap: `${theme.spacing(1)}px`,
  },
  dialogValue: {
    display: 'flex',
    flexDirection: 'column',
    gap: `${theme.spacing(2)}px`,
    width: '100%',
  },
  filterCard: {
    display: 'flex',
    flexDirection: 'column',
    gap: `${theme.spacing(2)}px`,
  },
  filterCardContent: {
    display: 'flex',
    flexDirection: 'column',
    gap: `${theme.spacing(2)}px`,
  },
  filterCardHeader: {
    alignItems: 'center',
    display: 'flex',
    justifyContent: 'space-between',
    overflow: 'hidden',
  },
  filterCardTitle: {
    alignItems: 'center',
    display: 'flex',
    gap: `${theme.spacing(1)}px`,
  },
  filterCardType: {
    color: theme.typography.color.secondary,
  },
  filtersContainer: {
    display: 'flex',
    flexDirection: 'column',
    gap: `${theme.spacing(2)}px`,
  },
  filtersHeader: {
    alignItems: 'center',
    display: 'flex',
    justifyContent: 'space-between',
  },
}));

function CategoryFilters() {
  const { theme } = useTheme();
  const { classes } = getClasses(theme);
  const { formatMessage } = useIntl();
  const { categoryId } = useParams<{ categoryId: string }>();
  const isSmallMobile = useIsSmallMobile();

  const {
    category,
    containerState,
    editableFilter,
    filterFields,
    filters,
    handleCloseAfterCreateFilterSuccessAlert,
    handleCloseAfterDeleteFilterSuccessAlert,
    handleCloseAfterUpdateFilterSuccessAlert,
    handleCloseCreateUpdateDialog,
    handleCloseDeleteDialog,
    handleConfirmDeleteFilter,
    handleConfirmSaveFilter,
    handleEditableFieldChange,
    handleStartCreate,
    handleStartDelete,
    handleStartEdit,
    isFailed,
    isFetching,
    isFetchingDelete,
    isFetchingSave,
    showAfterCreateFilterSuccessAlert,
    showAfterDeleteFilterSuccessAlert,
    showAfterUpdateFilterSuccessAlert,
  } = useCategoryFilters(Number(categoryId));

  const getFilterTypeLabel = (type: FilterType) => {
    switch (type) {
      case FilterType.Checkbox:
        return formatMessage({ id: 'filter.type.checkbox' });
      case FilterType.Dropdown:
        return formatMessage({ id: 'filter.type.dropdown' });
      case FilterType.Price:
        return formatMessage({ id: 'filter.type.price' });
      case FilterType.Radio:
        return formatMessage({ id: 'filter.type.radio' });
      default:
        return '';
    }
  };

  return (
    <div className={classes.container}>
      <Breadcrumbs>
        <Breadcrumb
          label={formatMessage({ id: 'catalog' })}
          to={{ pathname: pagesURLs[pages.catalog] }}
          variant="link"
        />
        <Show condition={!!category.data}>
          <Breadcrumb
            label={category.data?.name || ''}
            variant="text"
          />
        </Show>
      </Breadcrumbs>

      <Show condition={category.isFetching}>
        <Loading variant="loading" />
      </Show>

      <Show condition={!category.isFetching}>
        <div className={classes.filtersHeader}>
          <Typography variant="title">
            {formatMessage({ id: 'catalog.filters.title' })}
          </Typography>
          <Button
            onClick={handleStartCreate}
            variant="primary"
          >
            <Typography color="inherit">
              {formatMessage({ id: 'catalog.filters.create' })}
            </Typography>
          </Button>
        </div>

        <Show condition={isFetching}>
          <Loading variant="loading" />
        </Show>

        <Show condition={!isFetching}>
          <div className={classes.filtersContainer}>
            {filters?.length === 0 ? (
              <Loading variant="noData">
                <Typography
                  align="center"
                  color="secondary"
                  variant="subtitle"
                >
                  {formatMessage({ id: 'loading.noData' })}
                </Typography>
              </Loading>
            ) : (
              filters?.map((filter: IFilter) => (
                <Card
                  key={filter.id}
                  customBackground={theme.card.background.paper}
                  variant="edit"
                >
                  <CardContent>
                    <div className={classes.filterCardHeader}>
                      <div className={classes.filterCardTitle}>
                        <Typography variant="subtitle">
                          {filter.name}
                        </Typography>
                        <Typography
                          color="secondary"
                          variant="caption"
                        >
                          {getFilterTypeLabel(filter.type)}
                        </Typography>
                      </div>
                      <div className={classes.actionsContainer}>
                        <IconButton
                          disableHoverSpace
                          onClick={() => handleStartEdit(filter)}
                        >
                          <IconEdit size={ICON_SIZE} />
                        </IconButton>
                        <IconButton
                          disableHoverSpace
                          onClick={() => handleStartDelete(filter.id)}
                        >
                          <IconDelete size={ICON_SIZE} />
                        </IconButton>
                      </div>
                    </div>
                  </CardContent>
                </Card>
              ))
            )}
          </div>
        </Show>
      </Show>

      {/* Create/Update Dialog */}
      <Dialog
        onClose={handleCloseCreateUpdateDialog}
        open={containerState.showCreateUpdateDialog}
      >
        <Card
          customBackground={theme.card.background.paper}
          variant="edit"
        >
          <CardTitle>
            <Typography variant="title">
              {formatMessage({
                id: containerState.editableFilterId
                  ? 'title.update'
                  : 'title.create',
              })}
            </Typography>
            <IconButton
              disableHoverSpace
              onClick={handleCloseCreateUpdateDialog}
            >
              <IconClose size={ICON_SIZE} />
            </IconButton>
          </CardTitle>
          <CardContent>
            <div className={classes.dialogContainer}>
              <div className={classes.dialogRow}>
                <div className={classes.dialogLabel}>
                  <Typography color="secondary">
                    <Colon>
                      {formatMessage({
                        id: 'filter.field.name',
                      })}
                    </Colon>
                  </Typography>
                  <Show
                    condition={containerState
                      .validationErrors
                      .includes(errorTypes.EMPTY_FIELD_NAME)}
                  >
                    <Tooltip
                      arrow
                      placement="top"
                      title={formatMessage({
                        id: `validationError.${
                          errorTypes.EMPTY_FIELD_NAME}`,
                      })}
                    >
                      <IconError
                        color={theme.colors.redDark}
                        size={ICON_SIZE}
                      />
                    </Tooltip>
                  </Show>
                </div>
                <div className={classes.dialogValue}>
                  <TextField
                    fullWidth
                    isError={containerState
                      .validationErrors
                      .includes(errorTypes.EMPTY_FIELD_NAME)}
                    maxLength={34}
                    onChange={({ target }) =>
                      handleEditableFieldChange({
                        fieldKey: filterFields.NAME,
                        value: target.value,
                      })}
                    value={editableFilter.name}
                  />
                </div>
              </div>
              <div className={classes.dialogRow}>
                <div className={classes.dialogLabel}>
                  <Typography color="secondary">
                    <Colon>
                      {formatMessage({
                        id: 'filter.field.type',
                      })}
                    </Colon>
                  </Typography>
                </div>
                <div className={classes.dialogValue}>
                  <Select
                    fullWidth
                    onChange={({ target }) =>
                      handleEditableFieldChange({
                        fieldKey: filterFields.TYPE,
                        value: target.value,
                      })}
                    value={editableFilter.type}
                  >
                    <MenuItem value={FilterType.Checkbox}>
                      {formatMessage({ id: 'filter.type.checkbox' })}
                    </MenuItem>
                    <MenuItem value={FilterType.Dropdown}>
                      {formatMessage({ id: 'filter.type.dropdown' })}
                    </MenuItem>
                    <MenuItem value={FilterType.Price}>
                      {formatMessage({ id: 'filter.type.price' })}
                    </MenuItem>
                    <MenuItem value={FilterType.Radio}>
                      {formatMessage({ id: 'filter.type.radio' })}
                    </MenuItem>
                  </Select>
                </div>
              </div>
            </div>
          </CardContent>
          <CardActions>
            <Button
              onClick={handleCloseCreateUpdateDialog}
              variant="secondary"
            >
              <Typography color="inherit">
                {formatMessage({ id: 'cancel' })}
              </Typography>
            </Button>
            <Button
              isLoading={isFetchingSave}
              onClick={handleConfirmSaveFilter}
              variant="primary"
            >
              <Typography color="inherit">
                {formatMessage({ id: 'save' })}
              </Typography>
            </Button>
          </CardActions>
        </Card>
      </Dialog>

      {/* Delete Dialog */}
      <Dialog
        onClose={handleCloseDeleteDialog}
        open={!!containerState.filterIdToDelete}
      >
        <Card>
          <CardTitle>
            <Typography variant="title">
              {formatMessage({ id: 'delete.title' })}
            </Typography>
            <IconButton
              disableHoverSpace
              onClick={handleCloseDeleteDialog}
            >
              <IconClose size={ICON_SIZE} />
            </IconButton>
          </CardTitle>
          <CardContent>
            <Typography color="secondary">
              {formatMessage({ id: 'delete.subtitle' })}
            </Typography>
          </CardContent>
          <CardActions>
            <Button
              onClick={handleCloseDeleteDialog}
              variant="secondary"
            >
              <Typography color="inherit">
                {formatMessage({ id: 'cancel' })}
              </Typography>
            </Button>
            <Button
              isLoading={isFetchingDelete}
              onClick={handleConfirmDeleteFilter}
              variant="primary"
            >
              <Typography color="inherit">
                {formatMessage({ id: 'delete' })}
              </Typography>
            </Button>
          </CardActions>
        </Card>
      </Dialog>

      <Snackbar
        autoHide
        onClose={handleCloseAfterCreateFilterSuccessAlert}
        open={showAfterCreateFilterSuccessAlert}
      >
        <Card variant="success">
          <CardContent>
            <Typography color="success">
              {formatMessage({
                id: 'snackbar.successCreateFilter',
              })}
            </Typography>
          </CardContent>
        </Card>
      </Snackbar>
      <Snackbar
        autoHide
        onClose={handleCloseAfterDeleteFilterSuccessAlert}
        open={showAfterDeleteFilterSuccessAlert}
      >
        <Card variant="success">
          <CardContent>
            <Typography color="success">
              {formatMessage({
                id: 'snackbar.successDeleteFilter',
              })}
            </Typography>
          </CardContent>
        </Card>
      </Snackbar>
      <Snackbar
        autoHide
        onClose={handleCloseAfterUpdateFilterSuccessAlert}
        open={showAfterUpdateFilterSuccessAlert}
      >
        <Card variant="success">
          <CardContent>
            <Typography color="success">
              {formatMessage({
                id: 'snackbar.successUpdateFilter',
              })}
            </Typography>
          </CardContent>
        </Card>
      </Snackbar>
    </div>
  );
}

export default CategoryFilters;
