import { Component } from '@angular/core';
import { MeterReadingService } from '../../services/meter-reading.service'

@Component({
  selector: 'app-meter-reading-upload',
  templateUrl: './meter-reading-upload.component.html',
  styleUrls: ['./meter-reading-upload.component.css'],
})
export class MeterReadingUploadComponent {
  selectedFile: File | null = null;
  responseMessage: string | null = null;

  constructor(private meterReadingService: MeterReadingService) {}

  onFileSelected(event: any): void {
    this.selectedFile = event.target.files[0];
  }

  onUpload(): void {
    if (!this.selectedFile) {
      this.responseMessage = 'Please select a file before uploading.';
      return;
    }

    this.meterReadingService.uploadMeterReadings(this.selectedFile).subscribe({
      next: (response) => {
        this.responseMessage = response.Message;
        console.log(response); // Handle additional response data
      },
      error: (err) => {
        this.responseMessage = 'An error occurred during upload.';
        console.error(err);
      },
    });
  }
}
