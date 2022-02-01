import { fetch as crossFetch } from 'cross-fetch';

export const fetch = async (
  info: RequestInfo,
  init?: RequestInit,
  refresh?: () => Promise<void>
) => {
  try {
    let response = await crossFetch(info, init);

    if (response.status !== 401) {
      return response;
    }

    if (response.status === 401 && refresh) {
      await refresh();
      response = await crossFetch(info, init);
    }

    return response;
  } catch (error) {
    throw new Error('Failed to fetch.');
  }
};
