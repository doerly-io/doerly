import React from 'react';
import CheckboxMUI from '@mui/material/Checkbox';
import useTheme from 'hooks/useTheme';

const Checkbox = ({
  checked = false,
  color,
  disabled = false,
  disableHoverSpace = false,
  onChange,
}: IProps) => {
  const { theme } = useTheme();
  return (
    <CheckboxMUI
      checked={checked}
      disabled={disabled}
      onChange={onChange}
      size="small"
      sx={{
        '&.Mui-checked': {
          color: color || theme.colors.cobalt,
        },
        color: color || theme.colors.cobalt,
        margin: disableHoverSpace ? `-${theme.spacing(1)}px` : '0px',
      }}
    />
  );
};

interface IProps {
  checked?: boolean,
  color?: string,
  disabled?: boolean,
  disableHoverSpace?: boolean,
  onChange?: (event: any) => void;
}

export default Checkbox;
