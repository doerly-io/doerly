const sizeTypes = {
  lg: 'lg',
  md: 'md',
  sm: 'sm',
  xl: 'xl',
  xs: 'xs',
};

const sizesToTypes = {
  [sizeTypes.xs]: 0,
  [sizeTypes.sm]: 600,
  [sizeTypes.md]: 900,
  [sizeTypes.lg]: 1200,
  [sizeTypes.xl]: 1536,
};

const forExport = {
  sizeTypes,
  sizesToTypes,
};

export default forExport;
