import { Component } from '@angular/core';
import { MeterReadingService } from '../../services/meter-reading.service'

@Component({
  selector: 'app-meter-reading-upload',
  templateUrl: './meter-reading-upload.component.html',
  styleUrls: ['./meter-reading-upload.component.css'],
})
export class MeterReadingUploadComponent {
  selectedFile: File | null = null;
  response: any;
  errorMessage: string | null = null; // To handle any errors

  constructor(private meterReadingService: MeterReadingService) {}

  onFileSelected(event: any): void {
    this.selectedFile = event.target.files[0];
  }

  onUpload(): void {
    if (!this.selectedFile) {
      this.errorMessage = 'Please select a file before uploading.';
      return;
    }

    this.meterReadingService.uploadMeterReadings(this.selectedFile).subscribe({
      next: (response) => {
        this.errorMessage = null;
        this.response = response;
        console.log(response);
      },
      error: (err) => {
        this.response = null;
        this.errorMessage = err.error.criticalError;
        console.error(err);
      },
    });
  }
}
