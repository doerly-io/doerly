import React from 'react';
import SvgIcon from '../SvgIcon';
import useTheme from 'hooks/useTheme';

/* eslint-disable max-len */
const Reporting = ({
  color = 'page',
  size = 32,
}: IProps) => {
  const { theme } = useTheme();
  const actualColor = theme.icon.color[color] || color;
  return (
    <SvgIcon
      style={{
        height: `${size}px`,
        width: `${size}px`,
      }}
      viewBox="0 0 512 512"
    >
      <path
        d="m384 85.333l85.334 85.333v256H42.667V85.333zM373.334 128h-288v256h341.333V181.333zm-224 42.666L149.333 320H384v21.334H128V170.666zm64 64v64h-42.667v-64zm64-42.666v106.666h-42.667V192zm64 64v42.666h-42.667V256z"
        fill={actualColor}
        fillRule="evenodd"
      />
    </SvgIcon>
  );
};

interface IProps {
  color?: 'header' | 'page' | string,
  size?: number,
}

export default Reporting;
