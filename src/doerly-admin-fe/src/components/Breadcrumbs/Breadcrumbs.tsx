import { makeStyles } from 'tss-react/mui';
import React from 'react';
import Typography from 'components/Typography';
import useTheme from 'hooks/useTheme';

const getClasses = makeStyles<any>()((_, theme: any) => ({
  container: {
    alignItems: 'center',
    display: 'flex',
    gap: `${theme.spacing(1)}px`,
  },
}));

function Breadcrumbs({
  children: inputChildren,
}: IProps) {
  const { theme } = useTheme();
  const { classes } = getClasses(theme);
  const children = Array.isArray(inputChildren)
    ? inputChildren
    : [inputChildren];
  return (
    <div className={classes.container}>
      {children.map((child, index) => (
        <>
          {index !== 0 && (
            <Typography variant="title">
              /
            </Typography>
          )}
          {child}
        </>
      ))}
    </div>
  );
}

interface IProps {
  children: React.ReactNode,
}

export default Breadcrumbs;
