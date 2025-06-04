import React from 'react';
import { makeStyles } from 'tss-react/mui';
import CircularProgress from 'components/CircularProgress';
import classNames from 'classnames';
import IconError from 'components/icons/Error';
import IconNoData from 'components/icons/NoData';
import useTheme from 'hooks/useTheme';

const getClasses = makeStyles<any>()((_, theme: any) => ({
  background_default: {
    backgroundColor: theme.loading.background,
  },
  background_inherit: {
    backgroundColor: 'inherit',
  },
  child: {
    padding: `0px ${theme.spacing(2)}px`,
  },
  container: {
    alignItems: 'center',
    display: 'flex',
    flexDirection: 'column',
    gap: `${theme.spacing(2)}px`,
    height: '100%',
    justifyContent: 'center',
    minHeight: '200px',
    width: '100%',
  },
}));

type TVariants = 'error' | 'loading' | 'noData';

type TBgVariants = 'default' | 'inherit';

type iconsTypesToTVariants = {
  [K in TVariants]: React.ReactNode;
};


const iconsToVariants = (size: number): iconsTypesToTVariants => ({
  error: (<IconError size={size} />),
  loading: (<CircularProgress size={size} />),
  noData: (<IconNoData size={size} />),
});

const makeArray = (input: any) => {
  if (!input) {
    return [];
  }
  return Array.isArray(input) ? input : [input];
};

function Loading({
  backgroundVariant = 'inherit',
  children: inputChildren,
  inline = false,
  size = 56,
  variant = 'loading',
}: IProps) {
  const { theme } = useTheme();
  const { classes } = getClasses(theme);
  const children = makeArray(inputChildren) as any;
  const icons = iconsToVariants(size);
  return (
    <div
      className={classNames(
        !inline && classes.container,
        classes[`background_${backgroundVariant}`]
      )}
    >
      {icons[variant]}
      {children.map((child: any) => (
        <div className={classes.child}>
          {child}
        </div>
      ))}
    </div>
  );
}

interface IProps {
  backgroundVariant?: TBgVariants,
  children?: React.ReactNode,
  inline?: boolean,
  size?: number,
  variant?: TVariants,
}

export default Loading;
