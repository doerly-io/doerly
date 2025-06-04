import { makeStyles } from 'tss-react/mui';
import React from 'react';
import InputAdornmentMui from '@mui/material/InputAdornment';
import TextFieldMui from '@mui/material/TextField';
import useTheme from 'hooks/useTheme';

type TTextVariants = 'capitalized'
  | 'caption'
  | 'default'
  | 'subtitle'
  | 'title';

const getClasses = makeStyles<any>()((_, theme: any) => ({
  capitalized: theme.typography.variants.capitalized,
  caption: theme.typography.variants.caption,
  default: theme.typography.variants.default,
  input: {
    '& input': {
      color: theme.typography.color.primary,
    },
    '& input::-webkit-outer-spin-button, & input::-webkit-inner-spin-button': {
      display: 'none',
    },
    '& input:focus': {
      color: theme.typography.color.primary,
    },
    '& input[type=number]': {
      MozAppearance: 'textfield',
    },
  },
  subtitle: theme.typography.variants.subtitle,
  title: theme.typography.variants.title,
}));

const positionOptions = {
  BOTTOM: 'bottom',
  CENTER: 'center',
  TOP: 'top',
};

const adornmentPositionToFlexVariant = {
  [positionOptions.BOTTOM]: 'flex-end',
  [positionOptions.CENTER]: 'center',
  [positionOptions.TOP]: 'flex-start',
};

const TextField = ({
  AdornmentStart,
  AdornmentEnd,
  adornmentPosition = 'center',
  autoFocus = false,
  color = 'success',
  disabled = false,
  focused = true,
  fullHeight = false,
  fullWidth = false,
  helperText,
  inputRef,
  inputType = 'text',
  isError = false,
  label,
  maxLength = 100,
  minRows,
  multiline = false,
  onBlur,
  onChange,
  onKeyDown,
  onSelect,
  placeholder,
  size,
  textVariant = 'default',
  value,
  variant = 'standard',
  ...rest
}: IProps) => {
  const { theme } = useTheme();
  const { classes } = getClasses(theme);

  return (
    <TextFieldMui
      {...rest}
      autoFocus={autoFocus}
      className={classes.input}
      color={color}
      disabled={disabled}
      error={isError}
      focused={focused}
      fullWidth={fullWidth}
      helperText={helperText}
      InputProps={{
        ...((rest as any).InputProps),
        classes: {
          input: classes[textVariant],
        },
        endAdornment: AdornmentEnd && (
          <InputAdornmentMui position="end">
            {AdornmentEnd}
          </InputAdornmentMui>
        ),
        startAdornment: AdornmentStart && (
          <InputAdornmentMui position="start">
            {AdornmentStart}
          </InputAdornmentMui>
        ),
        sx: {
          '&.MuiOutlinedInput-root': {
            padding: `0px ${theme.spacing(1)}px`,
          },
          alignItems: adornmentPositionToFlexVariant[adornmentPosition],
          height: fullHeight ? '100%' : undefined,
          marginTop: '0px !important',
        },
      }}
      inputProps={{
        ...((rest as any).inputProps || {}),
        maxLength,
      }}
      inputRef={inputRef}
      label={label}
      minRows={minRows}
      multiline={multiline}
      onBlur={onBlur}
      onChange={onChange}
      onKeyDown={onKeyDown}
      onSelect={onSelect}
      placeholder={placeholder}
      size={size}
      sx={{
        height: fullHeight ? '100%' : undefined,
      } as any}
      type={inputType}
      value={value}
      variant={variant}
    />
  );
};

interface IProps {
  AdornmentEnd?: any;
  AdornmentStart?: any;
  adornmentPosition?: 'top' | 'center' | 'bottom',
  autoFocus?: boolean,
  color?: 'primary' | 'secondary' | 'error' | 'info' | 'success' | 'warning';
  disabled?: boolean,
  focused?: boolean,
  fullHeight?: boolean,
  fullWidth?: boolean,
  helperText?: React.ReactNode;
  inputRef?: React.RefObject<HTMLTextAreaElement | HTMLInputElement>,
  inputType?: React.HTMLInputTypeAttribute,
  isError?: boolean;
  label?: React.ReactNode;
  maxLength?: number;
  minRows?: number | string;
  multiline?: boolean,
  onBlur?: (event: any) => void;
  onChange?: (event: any) => void;
  onKeyDown?: (event: any) => void,
  onSelect?: (event: any) => void;
  placeholder?: string;
  size?: 'medium' | 'small';
  textVariant?: TTextVariants,
  value?: any;
  variant?: 'filled' | 'outlined' | 'standard';
}
export default TextField;
