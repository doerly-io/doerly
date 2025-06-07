import React from 'react';
import AccordionMui from '@mui/material/Accordion';
import useTheme from 'hooks/useTheme';

const Accordion = ({
  expanded,
  children,
  onChange,
}: IProps) => {
  const { theme } = useTheme();
  return (
    <AccordionMui
      expanded={expanded}
      onChange={onChange}
      sx={{
        backgroundColor: theme.card.background.paper,
      }}
    >
      {children}
    </AccordionMui>
  );
};

interface IProps {
  children: React.ReactNode[],
  expanded?: boolean,
  onChange?: (event: React.SyntheticEvent, expanded: boolean) => void,
}

export default Accordion;
