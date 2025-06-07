import React from 'react';
import { makeStyles } from 'tss-react/mui';
import Typography from 'components/Typography';
import TreeNode from './TreeNode';
import { IGetCategoryResponse } from 'api/catalog/model';
import useTheme from 'hooks/useTheme';
import Card from 'components/Card';
import { useIntl } from 'react-intl';

const getClasses = makeStyles<any>()((_, theme: any) => ({
  container: {
    display: 'flex',
    flexDirection: 'column',
    gap: `${theme.spacing(2)}px`,
  },
  emptyState: {
    color: theme.typography.color.secondary,
    padding: `${theme.spacing(4)}px`,
    textAlign: 'center',
  },
  header: {
    alignItems: 'center',
    display: 'flex',
    justifyContent: 'space-between',
    padding: `${theme.spacing(2)}px`,
  },
  treeContainer: {
    padding: `${theme.spacing(1)}px`,
  },
}));

interface CategoryTreeProps {
  categories: IGetCategoryResponse[];
  onAddCategory?: (parentId: number) => void;
  onEditCategory?: (category: IGetCategoryResponse) => void;
  onDeleteCategory?: (categoryId: number) => void;
  onToggleEnabled?: (categoryId: number, isEnabled: boolean) => void;
}

const CategoryTree: React.FC<CategoryTreeProps> = ({
  categories,
  onAddCategory,
  onEditCategory,
  onDeleteCategory,
  onToggleEnabled,
}) => {
  const { theme } = useTheme();
  const { classes } = getClasses(theme);
  const { formatMessage } = useIntl();

  return (
    <Card>
      <div className={classes.treeContainer}>
        {categories.length === 0 ? (
          <div className={classes.emptyState}>
            <Typography variant="subtitle">
              {formatMessage({ id: 'catalog.noCategories' })}
            </Typography>
            <Typography variant="subtitle">
              {formatMessage({ id: 'catalog.addCategory' })}
            </Typography>
          </div>
        ) : (
          categories.map((category) => (
            <TreeNode
              key={category.id}
              category={category}
              onAdd={onAddCategory}
              onEdit={onEditCategory}
              onDelete={onDeleteCategory}
              onToggleEnabled={onToggleEnabled}
            />
          ))
        )}
      </div>
    </Card>
  );
};

export default CategoryTree;
