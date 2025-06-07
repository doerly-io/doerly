import React from 'react';
import { makeStyles } from 'tss-react/mui';
import { useIntl } from 'react-intl';
import { useSelector } from 'react-redux';
import Breadcrumbs from 'components/Breadcrumbs';
import Breadcrumb from 'components/Breadcrumb';
import Button from 'components/Button';
import Card from 'components/Card';
import CardContent from 'components/CardContent';
import Loading from 'components/Loading';
import Typography from 'components/Typography';
import useTheme from 'hooks/useTheme';
import CardActions from 'components/CardActions';
import useLogin, { errorTypes } from 'pages/login/hooks/useLogin';
import TextField from 'components/TextField';
import Show from 'components/Show';
import Tooltip from 'components/Tooltip';
import IconError from 'components/icons/Error';
import Snackbar from 'components/Snackbar';
import CardTitle from 'components/CardTitle';


const ICON_SIZE = 24;

const getClasses = makeStyles<any>()((_, theme: any) => ({
  container: {
    display: 'flex',
    flexDirection: 'column',
    gap: `${theme.spacing(2)}px`,
  },
  formContainer: {
    display: 'flex',
    flexDirection: 'column',
    gap: `${theme.spacing(1)}px`,
    overflowY: 'hidden',
  },
  formRow: {
    alignItems: 'center',
    display: 'flex',
    gap: `${theme.spacing(2)}px`,
    height: '32px',
  },
  login: {
    alignItems: 'center',
    display: 'flex',
    flexDirection: 'column',
    height: '150px',
    justifyContent: 'center',
  },
  rowLabel: {
    alignItems: 'center',
    display: 'flex',
    flex: 1,
    gap: `${theme.spacing(1)}px`,
  },
  rowValue: {
    alignItems: 'center',
    display: 'flex',
    flex: 2,
    gap: `${theme.spacing(1)}px`,
    width: '50%',
  },
}));

function Login() {
  const { formatMessage } = useIntl();
  const { theme } = useTheme();
  const { classes } = getClasses(theme);

  const {
    email,
    errorsMsg,
    handleChangeEmail,
    handleChangePassword,
    handleSignIn,
    isFailedSignIn,
    isFetchingSignIn,
    password,
    validationErrors,
  } = useLogin();

  return (
    <div className={classes.container}>
      <Breadcrumbs>
        <Breadcrumb
          label={formatMessage({ id: 'signIn' })}
          variant="text"
        />
      </Breadcrumbs>
      <Card disablePaddings>
        {isFetchingSignIn && (
          <Loading/>
        )}
        {!isFetchingSignIn && (
          <Card>
            <CardContent>
              <div className={classes.formContainer}>
                <div className={classes.formRow}>
                  <div className={classes.rowLabel}>
                    <Typography color="secondary">
                      {formatMessage({ id: 'label.email' })}
                    </Typography>
                    <Show
                      condition={validationErrors
                        .includes(errorTypes.EMPTY_FIELD_LOGIN)}
                    >
                      <Tooltip
                        arrow
                        placement="top"
                        title={formatMessage({
                          id: `validationError.${
                            errorTypes.EMPTY_FIELD_LOGIN}`,
                        })}
                      >
                        <IconError
                          color={theme.colors.redDark}
                          size={ICON_SIZE}
                        />
                      </Tooltip>
                    </Show>
                  </div>
                  <div className={classes.rowValue}>
                    <TextField
                      fullWidth
                      inputType="email"
                      isError={validationErrors
                        .includes(errorTypes.EMPTY_FIELD_LOGIN)}
                      value={email}
                      onChange={(({ target }) =>
                        handleChangeEmail(target.value))}
                      placeholder={formatMessage({ id: 'placeholder.email' })}
                    />
                  </div>
                </div>
                <div className={classes.formRow}>
                  <div className={classes.rowLabel}>
                    <Typography color="secondary">
                      {formatMessage({ id: 'label.password' })}
                    </Typography>
                    <Show
                      condition={validationErrors
                        .includes(errorTypes.EMPTY_FIELD_PASSWORD)}
                    >
                      <Tooltip
                        arrow
                        placement="top"
                        title={formatMessage({
                          id: `validationError.${
                            errorTypes.EMPTY_FIELD_PASSWORD}`,
                        })}
                      >
                        <IconError
                          color={theme.colors.redDark}
                          size={ICON_SIZE}
                        />
                      </Tooltip>
                    </Show>
                  </div>
                  <div className={classes.rowValue}>
                    <TextField
                      fullWidth
                      inputType="password"
                      isError={validationErrors
                        .includes(errorTypes.EMPTY_FIELD_PASSWORD)}
                      onChange={(({ target }) =>
                        handleChangePassword(target.value))}
                      value={password}
                      placeholder={
                        formatMessage({ id: 'placeholder.password' })
                      }
                    />
                  </div>
                </div>
              </div>
            </CardContent>
            <CardActions>
              <Button
                onClick={() => handleSignIn()}
                variant="primary"
              >
                <Typography color="inherit">
                  {formatMessage({ id: 'signIn' })}
                </Typography>
              </Button>
            </CardActions>
          </Card>
        )}
      </Card>
      <Snackbar
        autoHide
        open={!!errorsMsg && isFailedSignIn}
      >
        <Card variant="error">
          <CardTitle>
            <Typography color="error">
              { errorsMsg }
            </Typography>
          </CardTitle>
        </Card>
      </Snackbar>
    </div>
  );
}

export default Login;
