export interface FormActionsProps {
  submitLabel: string;
  loadingLabel: string;
  isLoading: boolean;
  isDisabled?: boolean;
  onCancel: () => void;
}
