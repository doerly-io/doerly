import CircularProgressMUI from '@mui/material/CircularProgress';
import React from 'react';
import useTheme from 'hooks/useTheme';

function CircularProgress({
  size = 24,
}: IProps) {
  const { theme } = useTheme();
  return (
    <CircularProgressMUI
      size={size}
      sx={{
        colorPrimary: theme.circularProgress.color,
      }}
      thickness={3}
    />
  );
}

interface IProps {
  size?: number
}

export default CircularProgress;
