import React from 'react';
import { Highlight } from 'react-highlighter-ts';
import { makeStyles } from 'tss-react/mui';
import useTheme from 'hooks/useTheme';


const getClasses = makeStyles<any>()((_, theme: any) => ({
  matchClass: {
    backgroundColor: theme.colors.secondary,
  },
}));

const HighlightText = ({
  children,
  search,
}: IProps) => {
  const { theme } = useTheme();
  const { classes } = getClasses(theme);

  return (
    <Highlight
      matchClass={classes.matchClass}
      matchElement="span"
      search={search}
      placeholder=""
      onPointerEnterCapture={() => ({})}
      onPointerLeaveCapture={() => ({})}
    >
      {children}
    </Highlight>
  );
};

interface IProps {
  children: React.ReactNode,
  search: RegExp | string,
}

export default HighlightText;
