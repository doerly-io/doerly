import React from 'react';
import ChipMui from '@mui/material/Chip';
import useTheme from 'hooks/useTheme';

const Chip = ({
  color = 'default',
  disabled = false,
  label,
  onClick,
}: IProps) => {
  const { theme } = useTheme();
  return (
    <ChipMui
      disabled={disabled}
      label={label}
      onClick={onClick}
      size="small"
      sx={{
        '&.MuiChip-clickable:hover': {
          opacity: 0.8,
        },
        '&:hover': {
          opacity: 0.8,
        },
        backgroundColor: color,
        borderRadius: `${theme.spacing(0.5)}px`,
      }}
    />
  );
};

interface IProps {
  color?: string,
  disabled?: boolean,
  label: string,
  onClick?: (event: any) => void,
}

export default Chip;
