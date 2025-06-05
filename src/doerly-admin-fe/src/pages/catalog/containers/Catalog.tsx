import { makeStyles } from 'tss-react/mui';
import { useIntl } from 'react-intl';
import Typography from 'components/Typography';
import Breadcrumbs from 'components/Breadcrumbs';
import Breadcrumb from 'components/Breadcrumb';
import useTheme from 'hooks/useTheme';
import useCatalog, { errorTypes } from '../hooks/useCatalog';
import Loading from 'components/Loading';
import Show from 'components/Show';
import { IGetCategoryResponse } from 'api/catalog/model';
import CategoryTree from 'pages/catalog/components/CategoryTree';
import Dialog from 'components/Dialog';
import Card from 'components/Card';
import CardTitle from 'components/CardTitle';
import IconButton from 'components/IconButton';
import CardContent from 'components/CardContent';
import CardActions from 'components/CardActions';
import IconClose from 'components/icons/Close';
import Button from 'components/Button';
import TextField from 'components/TextField';
import Colon from 'components/Colon';
import Tooltip from 'components/Tooltip';
import IconError from 'components/icons/Error';
import Snackbar from 'components/Snackbar';
import Checkbox from 'components/Checkbox';
import useIsMobile, { useIsSmallMobile } from 'hooks/useIsMobile';

const getClasses = makeStyles<any>()((_, theme: any) => ({
  actionsContainer: {
    alignItems: 'center',
    display: 'flex',
    gap: `${theme.spacing(3)}px`,
  },
  actionsContainerMobile: {
    display: 'flex',
    flexDirection: 'column',
    gap: `${theme.spacing(2)}px`,
  },
  actionsItem: {
    alignItems: 'center',
    display: 'flex',
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
}));

const ICON_SIZE = 20;

function Catalog() {
  const { theme } = useTheme();
  const { classes } = getClasses(theme);
  const { formatMessage } = useIntl();
  const isSmallMobile = useIsSmallMobile();

  const {
    categories,
    categoryFields,
    containerState,
    editableCategory,
    handleCloseAfterCreateCategorySuccessAlert,
    handleCloseAfterDeleteCategorySuccessAlert,
    handleCloseAfterUpdateCategorySuccessAlert,
    handleCloseCreateUpdateDialog,
    handleCloseDeleteDialog,
    handleConfirmDeleteCategory,
    handleConfirmSaveCategory,
    handleEditableFieldChange,
    handleStartCreate,
    handleStartDelete,
    handleStartEdit,
    handleToggleEnabled,
    handleToggleShowDeletedCategories,
    handleToggleShowDisabledCategories,
    isFailed,
    isFetching,
    isFetchingSave,
    isFetchingDelete,
    showAfterCreateCategorySuccessAlert,
    showAfterDeleteCategorySuccessAlert,
    showAfterUpdateCategorySuccessAlert,
  } = useCatalog();

  return (
    <div className={classes.container}>
      <Breadcrumbs>
        <Breadcrumb
          label={formatMessage({ id: 'catalog' })}
          variant="text"
        />
      </Breadcrumbs>
      <div className={!isSmallMobile
        ? classes.actionsContainer
        : classes.actionsContainerMobile
      }>
        <div className={classes.actionsItem}>
          <Typography color="secondary">
            {formatMessage({ id: 'catalog.showDisabledCategories' })}
          </Typography>
          <Checkbox
            checked={containerState.showDisabledCategories}
            onChange={handleToggleShowDisabledCategories}
          />
        </div>
        <div className={classes.actionsItem}>
          <Typography color="secondary">
            {formatMessage({ id: 'catalog.showDeletedCategories' })}
          </Typography>
          <Checkbox
            checked={containerState.showDeletedCategories}
            onChange={handleToggleShowDeletedCategories}
          />
        </div>
      </div>
      <Show condition={isFetching}>
        <Loading variant='loading' />
      </Show>
      <Show condition={!isFetching}>
        <CategoryTree
          categories={categories || []}
          onAddCategory={handleStartCreate}
          onEditCategory={handleStartEdit}
          onDeleteCategory={handleStartDelete}
          onToggleEnabled={handleToggleEnabled}
        />
      </Show>

      {/* Create/Update Dialog */}
      <Dialog
        onClose={handleCloseCreateUpdateDialog}
        open={containerState.showCreateUpdateDialog}
      >
        <Card
          customBackground={theme.card.background.paper}
          variant='edit'
        >
          <CardTitle>
            <Typography variant="title">
              {formatMessage({
                id: containerState.editableCategoryId
                  ? 'title.update'
                  : 'title.create',
              })}
            </Typography>
            <IconButton
              disableHoverSpace
              onClick={handleCloseCreateUpdateDialog}
            >
              <IconClose size={ICON_SIZE}/>
            </IconButton>
          </CardTitle>
          <CardContent>
            <div className={classes.dialogContainer}>
              <div className={classes.dialogRow}>
                <div className={classes.dialogLabel}>
                  <Typography color="secondary">
                    <Colon>
                      {formatMessage({
                        id: 'category.field.name',
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
                        fieldKey: categoryFields.NAME,
                        value: target.value,
                      })}
                    value={editableCategory.name}
                  />
                </div>
              </div>
              <div className={classes.dialogRow}>
                <div className={classes.dialogLabel}>
                  <Typography color="secondary">
                    <Colon>
                      {formatMessage({
                        id: 'category.field.description',
                      })}
                    </Colon>
                  </Typography>
                  <Show
                    condition={containerState
                      .validationErrors
                      .includes(errorTypes.EMPTY_FIELD_DESCRIPTION)}
                  >
                    <Tooltip
                      arrow
                      placement="top"
                      title={formatMessage({
                        id: `validationError.${
                          errorTypes.EMPTY_FIELD_DESCRIPTION}`,
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
                      .includes(errorTypes.EMPTY_FIELD_DESCRIPTION)}
                    maxLength={34}
                    onChange={({ target }) =>
                      handleEditableFieldChange({
                        fieldKey: categoryFields.DESCRIPTION,
                        value: target.value,
                      })}
                    value={editableCategory.description}
                  />
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
              onClick={handleConfirmSaveCategory}
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
        open={!!containerState.categoryIdToDelete}
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
              <IconClose size={24}/>
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
              onClick={handleConfirmDeleteCategory}
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
        onClose={handleCloseAfterCreateCategorySuccessAlert}
        open={showAfterCreateCategorySuccessAlert}
      >
        <Card variant="success">
          <CardContent>
            <Typography color="success">
              {formatMessage({
                id: 'snackbar.successCreateCategory',
              })}
            </Typography>
          </CardContent>
        </Card>
      </Snackbar>
      <Snackbar
        autoHide
        onClose={handleCloseAfterDeleteCategorySuccessAlert}
        open={showAfterDeleteCategorySuccessAlert}
      >
        <Card variant="success">
          <CardContent>
            <Typography color="success">
              {formatMessage({
                id: 'snackbar.successDeleteCategory',
              })}
            </Typography>
          </CardContent>
        </Card>
      </Snackbar>
      <Snackbar
        autoHide
        onClose={handleCloseAfterUpdateCategorySuccessAlert}
        open={showAfterUpdateCategorySuccessAlert}
      >
        <Card variant="success">
          <CardContent>
            <Typography color="success">
              {formatMessage({
                id: 'snackbar.successUpdateCategory',
              })}
            </Typography>
          </CardContent>
        </Card>
      </Snackbar>
    </div>
  );
}

export default Catalog;
