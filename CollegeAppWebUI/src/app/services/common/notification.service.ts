import { Injectable } from "@angular/core";
import swal from 'sweetalert2';
declare const $: any;

@Injectable({
  providedIn: 'root'
})
export class NotificationService {

  successMessage(message: string, title: string) {
    this.showSwal('success-message', message, title);
  }
  errorMessage(message: string, title: string) {
    this.showSwal('error-message', message, title);
  }
  htmlMessage(message: string, title: string) {
    this.showSwal('custom-html', message, title);
  }
  deleteConfirmation(message: string, title: string, okcallback: any) {
    this.showSwal('warning-message-and-confirmation', message, title, okcallback);
  }
  showSwal(type: any, message?: any, title?: any, callback?: any) {
    if (type == 'success-message') {
      swal.fire({
        title: title,
        text: message,
        buttonsStyling: false,
        customClass: {
          confirmButton: "btn btn-success",
        },
        icon: "success"
      });

    } else if (type == 'error-message') {
      swal.fire({
        title: title,
        text: message,
        buttonsStyling: false,
        customClass: {
          confirmButton: "btn btn-success",
        },
        icon: "error"
      });

    } else if (type == 'warning-message-and-confirmation') {
      swal.fire({
        title: title,
        text: message,
        icon: 'question',
        showCancelButton: true,
        customClass: {
          confirmButton: 'btn btn-success',
          cancelButton: 'btn btn-danger',
        },
        confirmButtonText: 'Yes',
        buttonsStyling: false
      }).then((result: any) => {
        if (result.value) {
          if (callback)
            callback();
        }
      });
    }
  }
}
