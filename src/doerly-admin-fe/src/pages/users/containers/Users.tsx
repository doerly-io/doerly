import Typography from 'components/Typography';
import React from 'react';
import { useIntl } from 'react-intl';
import { makeStyles } from 'tss-react/mui';
import Accordion from 'components/Accordion';
import AccordionDetails from 'components/AccordionDetails';
import AccordionSummary from 'components/AccordionSummary';
import Breadcrumbs from 'components/Breadcrumbs';
import Breadcrumb from 'components/Breadcrumb';
import Card from 'components/Card';
import CardContent from 'components/CardContent';
import Colon from 'components/Colon';
import useTheme from 'hooks/useTheme';
import useIsMobile, { useIsSmallMobile } from 'hooks/useIsMobile';
import useChangePage from 'hooks/useChangePage';
import useLocationSearch from 'hooks/useLocationSearch';
import useUsers from 'pages/users/hooks/useUsers';
import Show from 'components/Show';
import Loading from 'components/Loading';
import Divider from 'components/Divider';
import IconChevronDown from 'components/icons/ChevronDown';
import getInitials from 'utils/stringHelper';
import { ESex, ELanguageProficiencyLevel } from 'api/profiles/model';
import Link from 'components/Link';
import IconUpload from 'components/icons/Upload';
import TextField from 'components/TextField';
import IconSearch from 'components/icons/Search';
import { errorTypes } from 'pages/login/hooks/useLogin';
import IconError from 'components/icons/Error';
import Tooltip from 'components/Tooltip';
import HighlightText from 'components/HighlightText';

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
    flexShrink: 0,
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
    flexShrink: 0,
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
    minWidth: 0,
    overflow: 'hidden',
  },
  detailsContainer: {
    display: 'flex',
    flexDirection: 'column',
    gap: `${theme.spacing(1)}px`,
    marginBottom: `${theme.spacing(2)}px`,
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
  listContainer: {
    display: 'flex',
    flexDirection: 'column',
    gap: `${theme.spacing(1)}px`,
    marginTop: `${theme.spacing(1)}px`,
  },
  listItem: {
    alignItems: 'center',
    display: 'flex',
    gap: `${theme.spacing(1)}px`,
    padding: `${theme.spacing(0.5)}px 0`,
  },
  nameContainer: {
    display: 'flex',
    flexDirection: 'column',
    minWidth: 0,
    overflow: 'hidden',
  },
  sectionTitle: {
    marginBottom: `${theme.spacing(1)}px`,
    marginTop: `${theme.spacing(2)}px`,
  },
  summaryCell: {
    flex: 1,
    minWidth: 0,
    overflow: 'hidden',
  },
  summaryContainer: {
    alignItems: 'center',
    display: 'flex',
    gap: `${theme.spacing(1)}px`,
    justifyContent: 'space-between',
    minWidth: 0,
    width: '100%',
  },
  tableCell: {
    maxWidth: 'fit-content',
  },
}));

const ICON_SIZE = 24;

const Users = () => {
  const { theme } = useTheme();
  const { classes } = getClasses(theme);
  const { formatMessage } = useIntl();
  const isMobile = useIsMobile();
  const isSmallMobile = useIsSmallMobile();
  const changePage = useChangePage();
  const locationSearch = useLocationSearch();

  const {
    containerState,
    debouncedSearchText,
    handleSearchTextChange,
    isFetching,
    profiles,
  } = useUsers();

  const getSexLabel = (sex: ESex) => {
    switch (sex) {
      case ESex.Male:
        return formatMessage({ id: 'label.profile.sex.male' });
      case ESex.Female:
        return formatMessage({ id: 'label.profile.sex.female' });
      default:
        return formatMessage({ id: 'label.profile.sex.none' });
    }
  };

  const getLanguageLevelLabel = (level: ELanguageProficiencyLevel) => {
    return formatMessage({ id: `label.profile.languageLevel.${level}` });
  };

  return (
    <div className={classes.container}>
      <Breadcrumbs>
        <Breadcrumb
          label={formatMessage({ id: 'users' })}
          variant="text"
        />
      </Breadcrumbs>
      <div className={classes.actionsContainer}>
        <TextField
          AdornmentStart={<IconSearch size={ICON_SIZE}/>}
          AdornmentEnd={
            <Tooltip
              arrow
              placement="top"
              title={formatMessage({
                id: 'search.tooltip',
              })}
            >
              <IconError
                color={theme.colors.cobalt}
                size={ICON_SIZE}
              />
            </Tooltip>
          }
          onChange={({ target }) => handleSearchTextChange(target.value)}
          placeholder={formatMessage({ id: 'search' })}
          size="small"
          value={containerState.searchText}
          variant="outlined"
        />
      </div>
      <Show condition={isFetching}>
        <Loading variant='loading' />
      </Show>
      <Show condition={!isFetching}>
        <div>
          {profiles?.map((profile) => (
            <>
              <Accordion key={profile.id}>
                <AccordionSummary
                  expandIcon={<IconChevronDown size={ICON_SIZE}/>}
                >
                  <div className={classes.summaryContainer}>
                    <div className={classes.summaryCell}>
                      <div className={classes.customerContainer}>
                        {profile.imageUrl ? (
                          <img
                            src={profile.imageUrl}
                            alt={`${profile.firstName} ${profile.lastName}`}
                            className={classes.avatar}
                          />
                        ) : (
                          <div className={classes.avatarPlaceholder}>
                            {getInitials(profile.firstName, profile.lastName)}
                          </div>
                        )}
                        <div className={classes.nameContainer}>
                          <HighlightText search={debouncedSearchText}>
                            <Typography noWrap>
                              {profile.firstName}
                            </Typography>
                            <Typography noWrap>
                              {profile.lastName}
                            </Typography>
                          </HighlightText>
                        </div>
                      </div>
                    </div>
                    <Show condition={!isSmallMobile}>
                      <div className={classes.summaryCell}>
                        <Typography noWrap>
                          {formatMessage({ id: 'profile.createdOn' })}
                        </Typography>
                        <Typography
                          color="secondary"
                          noWrap
                          variant="subtitle"
                        >
                          {new Date(profile.dateCreated)
                            .toLocaleDateString() + ', '}
                          {new Date(profile.dateCreated)
                            .toLocaleTimeString()}
                        </Typography>
                      </div>
                    </Show>
                    <Show condition={!isMobile}>
                      <div className={classes.summaryCell}>
                        <Typography noWrap>
                          {formatMessage({ id: 'profile.lastModified' })}
                        </Typography>
                        <Typography
                          color="secondary"
                          noWrap
                          variant="subtitle"
                        >
                          {new Date(profile.lastModifiedDate)
                            .toLocaleDateString() + ', '}
                          {new Date(profile.lastModifiedDate)
                            .toLocaleTimeString()}
                        </Typography>
                      </div>
                    </Show>
                  </div>
                </AccordionSummary>
                <Divider color={theme.colors.cobalt}/>
                <AccordionDetails disablePadding>
                  <Card>
                    <CardContent>
                      <div className={classes.detailsContainer}>
                        {/* First Name */}
                        <div className={isMobile
                          ? classes.detailsRowMobile
                          : classes.detailsRow
                        }>
                          <div className={classes.detailsLabel}>
                            <Typography color="secondary">
                              <Colon>
                                {formatMessage({
                                  id: 'label.profile.firstName',
                                })}
                              </Colon>
                            </Typography>
                          </div>
                          <div className={isMobile
                            ? classes.detailsValueValueMobile
                            : classes.detailsValue
                          }>
                            <Typography wordBreak="break-word">
                              {profile.firstName}
                            </Typography>
                          </div>
                        </div>

                        {/* Last Name */}
                        <div className={isMobile
                          ? classes.detailsRowMobile
                          : classes.detailsRow
                        }>
                          <div className={classes.detailsLabel}>
                            <Typography color="secondary">
                              <Colon>
                                {formatMessage({
                                  id: 'label.profile.lastName',
                                })}
                              </Colon>
                            </Typography>
                          </div>
                          <div className={isMobile
                            ? classes.detailsValueValueMobile
                            : classes.detailsValue
                          }>
                            <Typography wordBreak="break-word">
                              {profile.lastName}
                            </Typography>
                          </div>
                        </div>

                        {/* Date of Birth */}
                        <div className={isMobile
                          ? classes.detailsRowMobile
                          : classes.detailsRow
                        }>
                          <div className={classes.detailsLabel}>
                            <Typography color="secondary">
                              <Colon>
                                {formatMessage({
                                  id: 'label.profile.dateOfBirth',
                                })}
                              </Colon>
                            </Typography>
                          </div>
                          <div className={isMobile
                            ? classes.detailsValueValueMobile
                            : classes.detailsValue
                          }>
                            <Typography>
                              {profile.dateOfBirth
                                ? new Date(profile.dateOfBirth)
                                  .toLocaleDateString()
                                : '-'}
                            </Typography>
                          </div>
                        </div>

                        {/* Sex */}
                        <div className={isMobile
                          ? classes.detailsRowMobile
                          : classes.detailsRow
                        }>
                          <div className={classes.detailsLabel}>
                            <Typography color="secondary">
                              <Colon>
                                {formatMessage({ id: 'label.profile.sex' })}
                              </Colon>
                            </Typography>
                          </div>
                          <div className={
                            isMobile
                              ? classes.detailsValueValueMobile
                              : classes.detailsValue
                          }>
                            <Typography>
                              {getSexLabel(profile.sex)}
                            </Typography>
                          </div>
                        </div>

                        {/* Bio */}
                        <div className={isMobile
                          ? classes.detailsRowMobile
                          : classes.detailsRow
                        }>
                          <div className={classes.detailsLabel}>
                            <Typography color="secondary">
                              <Colon>
                                {formatMessage({ id: 'label.profile.bio' })}
                              </Colon>
                            </Typography>
                          </div>
                          <div className={isMobile
                            ? classes.detailsValueValueMobile
                            : classes.detailsValue
                          }>
                            <Typography wordBreak="break-word">
                              {profile.bio || '-'}
                            </Typography>
                          </div>
                        </div>

                        {/* CV URL */}
                        <div className={isMobile
                          ? classes.detailsRowMobile
                          : classes.detailsRow
                        }>
                          <div className={classes.detailsLabel}>
                            <Typography color="secondary">
                              <Colon>
                                {formatMessage({ id: 'label.profile.cvUrl' })}
                              </Colon>
                            </Typography>
                          </div>
                          <div className={isMobile
                            ? classes.detailsValueValueMobile
                            : classes.detailsValue
                          }>
                            {profile.cvUrl ? (
                              <>
                                <Typography
                                  color="paper"
                                >
                                  {formatMessage({
                                    id: 'label.profile.downloadCV',
                                  })}
                                </Typography>
                                <Link
                                  href={profile.cvUrl}
                                  target="_blank"
                                >
                                  <IconUpload size={ICON_SIZE}/>
                                </Link>
                              </>
                            ) : (
                              <Typography>-</Typography>
                            )}
                          </div>
                        </div>

                        {/* Address */}
                        <div className={isMobile
                          ? classes.detailsRowMobile
                          : classes.detailsRow
                        }>
                          <div className={classes.detailsLabel}>
                            <Typography color="secondary">
                              <Colon>
                                {formatMessage({ id: 'label.profile.address' })}
                              </Colon>
                            </Typography>
                          </div>
                          <div className={isMobile
                            ? classes.detailsValueValueMobile
                            : classes.detailsValue
                          }>
                            <Typography>
                              {profile.address
                                ? profile.address.cityName +
                                ', ' +
                                profile.address.regionName
                                : '-'}
                            </Typography>
                          </div>
                        </div>

                        {/* Date Created */}
                        <div className={isMobile
                          ? classes.detailsRowMobile
                          : classes.detailsRow
                        }>
                          <div className={classes.detailsLabel}>
                            <Typography color="secondary">
                              <Colon>
                                {formatMessage({
                                  id: 'label.profile.dateCreated',
                                })}
                              </Colon>
                            </Typography>
                          </div>
                          <div className={isMobile
                            ? classes.detailsValueValueMobile
                            : classes.detailsValue
                          }>
                            <Typography>
                              {new Date(profile.dateCreated)
                                .toLocaleDateString()
                                + ', '}
                              {new Date(profile.dateCreated)
                                .toLocaleTimeString()}
                            </Typography>
                          </div>
                        </div>

                        {/* Last Modified Date */}
                        <div className={isMobile
                          ? classes.detailsRowMobile
                          : classes.detailsRow
                        }>
                          <div className={classes.detailsLabel}>
                            <Typography color="secondary">
                              <Colon>
                                {formatMessage({
                                  id: 'label.profile.lastModifiedDate',
                                })}
                              </Colon>
                            </Typography>
                          </div>
                          <div className={isMobile
                            ? classes.detailsValueValueMobile
                            : classes.detailsValue
                          }>
                            <Typography>
                              {new Date(profile.lastModifiedDate)
                                .toLocaleDateString()
                                + ', '}
                              {new Date(profile.lastModifiedDate)
                                .toLocaleTimeString()}
                            </Typography>
                          </div>
                        </div>
                      </div>

                      {/* Language Proficiencies Section */}
                      <Show condition={
                        profile.languageProficiencies.length > 0
                      }>
                        <Divider color={theme.colors.cobalt} />
                        <div className={classes.sectionTitle}>
                          <Typography variant="subtitle" color="secondary">
                            {formatMessage({
                              id: 'label.profile.languageProficiencies',
                            })}
                          </Typography>
                        </div>
                        <div className={classes.listContainer}>
                          {profile.languageProficiencies.map((langProf) => (
                            <div key={langProf.id} className={classes.listItem}>
                              <Typography color="secondary">•</Typography>
                              <Typography color="secondary">
                                {/*eslint-disable-next-line max-len*/}
                                {langProf.language.name} ({langProf.language.code})
                              </Typography>
                              <Typography color="secondary">-</Typography>
                              <Typography color="primary">
                                {getLanguageLevelLabel(langProf.level)}
                              </Typography>
                            </div>
                          ))}
                        </div>
                      </Show>

                      {/* Competences Section */}
                      <Show condition={profile.competences.length > 0}>
                        <Divider color={theme.colors.cobalt} />
                        <div className={classes.sectionTitle}>
                          <Typography variant="subtitle" color="primary">
                            {formatMessage({ id: 'label.profile.competences' })}
                          </Typography>
                        </div>
                        <div className={classes.listContainer}>
                          {profile.competences.map((competence) => (
                            <div
                              key={competence.id}
                              className={classes.listItem
                              }>
                              <Typography color="secondary">•</Typography>
                              <Typography>
                                {competence.categoryName}
                              </Typography>
                            </div>
                          ))}
                        </div>
                      </Show>
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
};

export default Users;
