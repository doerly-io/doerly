import React from 'react';
import ButtonMUI from '@mui/material/Button';
import CircularProgress from '../CircularProgress';
import useTheme from 'hooks/useTheme';

type TShapes = 'default' | 'round';

type TVariants = 'primary' | 'secondary' | 'text';

type TMUIVariants = 'contained' | 'outlined' | 'text';

type TVariantsToMUIVariants = {
  [K in TVariants]: TMUIVariants;
};

const variantsToMUIVariants: TVariantsToMUIVariants = {
  primary: 'contained',
  secondary: 'outlined',
  text: 'text',
};

function Button({
  children,
  disabled,
  endIcon,
  isLoading = false,
  onClick,
  shape = 'default',
  startIcon,
  variant = 'secondary',
}: IProps) {
  const { theme } = useTheme();
  const isRound = shape === 'round';
  const sx = {
    '&:hover': {
      backgroundColor: theme.button[variant].color.hover,
    },
    alignItems: isRound ? 'center' : undefined,
    backgroundColor: theme.button[variant].color.background,
    borderRadius: isRound ? '50%' : undefined,
    color: theme.button[variant].color.text,
    display: isRound ? 'flex' : undefined,
    height: isRound ? '48px !important' : '40px',

    justifyContent: isRound ? 'center' : undefined,
    minWidth: isRound ? 0 : undefined,
    padding: isRound ? 0 : undefined,
    width: isRound ? '48px' : undefined,

  };

  return (
    <>
      {isLoading && (
        <ButtonMUI
          disabled
          variant="contained"
          sx={sx}
        >
          <CircularProgress size={isRound ? 32 : 24} />
        </ButtonMUI>
      )}
      {!isLoading && (
        <ButtonMUI
          disabled={disabled}
          endIcon={endIcon}
          onClick={onClick}
          startIcon={startIcon}
          sx={sx}
          variant={variantsToMUIVariants[variant]}
        >
          {children}
        </ButtonMUI>
      )}
    </>
  );
}

interface IProps {
  children: React.ReactNode,
  disabled?: boolean,
  endIcon?: React.ReactNode,
  isLoading?: boolean,
  onClick: (input: any) => void,
  shape?: TShapes,
  startIcon?: React.ReactNode,
  variant?: TVariants,
}

export default Button;
