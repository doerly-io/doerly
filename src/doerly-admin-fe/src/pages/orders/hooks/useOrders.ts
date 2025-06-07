import { useEffect, useRef, useState } from 'react';
import apiOrders from 'api/orders/orders';
import apiProfile from 'api/profiles/profiles';
import useChangePage from 'hooks/useChangePage';
import useLocationSearch from 'hooks/useLocationSearch';
import {
  IOrder,
  IOrdersPagingResponse,
} from 'api/orders/model';
import { IProfileInfo } from 'api/profiles/model';

import * as paginationSizes from '../constants/paginationSizes';

const ALL = 'ALL';

const OrderStatusesColors: Record<number, string> = {
  1: '#34d399', // Placed
  2: '#fbbf24', // InProgress
  3: '#f87171', // AwaitingPayment
  4: '#60a5fa', // AwaitingConfirmation
  5: '#34d399', // Completed
  6: '#f87171', // Canceled
};

const useOrders = () => {
  const locationSearch = useLocationSearch();
  const changePage = useChangePage();
  const pageIndex = +locationSearch.pageIndex || 0;
  const pageSize = +locationSearch.pageSize || 1000;
  const customerId = locationSearch.customerId || ALL;
  const executorId = locationSearch.executorId || ALL;

  const componentDidMount = useRef(false);
  
  const [orders, setOrdersState] = useState<IOrdersState>({
    isFailed: false,
    isFetching: false,
    list: [] as Array<IOrder> | null,
    totalCount: 0,
    totalPages: 0,
  });

  const [profileInfos, setProfileInfos] = useState<Array<IProfileInfo>>([]);

  const fetchOrders = ({
    customerId,
    executorId,
    pageIndex,
    pageSize,
  }: any) => {
    apiOrders.getOrders({
      customerId: customerId === ALL ? undefined : Number(customerId),
      executorId: executorId === ALL ? undefined : Number(executorId),
      onFailed: () => setOrdersState((prevState) => ({
        ...prevState,
        isFailed: true,
        isFetching: false,
      })),
      onRequest: () => setOrdersState((prevState) => ({
        ...prevState,
        isFailed: false,
        isFetching: true,
      })),
      onSuccess: ({ value }: any) => {
        processOrdersResponse(value);
      },
      pageIndex: pageIndex + 1,
      pageSize,
    });
  };

  const fetchProfilesInfos = () => {
    apiProfile.getProfilesInfos({
      onFailed: () => setProfileInfos([]),
      onRequest: () => setProfileInfos([]),
      onSuccess: ({ value }: any) => {
        setProfileInfos(value);
      },
    });
  };

  const getExecutorDisplayName = (executorId: number | null | undefined) => {
    if (!executorId) {
      return '-';
    }
    const executor = profileInfos.find((info) => info.id === executorId);
    return executor ? `${executor.firstName} ${executor.lastName}` : '-';
  };

  const processOrdersResponse = (data: IOrdersPagingResponse) => {
    setOrdersState((prevState) => ({
      ...prevState,
      isFetching: false,
      list: data.orders,
      totalCount: data.total,
      totalPages: Math.ceil(data.total / (pageSize || paginationSizes.SMALL)),
    }));
  };

  useEffect(() => {
    fetchOrders({
      customerId: customerId === ALL ? undefined : customerId,
      executorId: executorId === ALL ? undefined : executorId,
      pageIndex,
      pageSize,
    });
  }, [
    customerId,
    executorId,
    pageIndex,
    pageSize,
  ]);

  useEffect(() => {
    componentDidMount.current = true;
    fetchProfilesInfos();
  }, []);

  return {
    customerId,
    executorId,
    getExecutorDisplayName,
    isFetching: orders.isFetching,
    orders: orders.list,
    profileInfos,
  };
};

interface IOrdersState {
  isFailed: boolean;
  isFetching: boolean;
  list: Array<IOrder> | null;
  totalCount: number;
  totalPages: number;
}

export {
  ALL,
  OrderStatusesColors
};

export default useOrders;
