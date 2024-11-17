import { ComponentFixture, TestBed } from '@angular/core/testing';
import { MeterReadingUploadComponent } from './meter-reading-upload.component';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { FileUploadComponent } from '../file-upload/file-upload.component';
import { ResponseDisplayComponent } from '../response-display/response-display.component';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { MeterReadingService } from '../../services/meter-reading.service';
import { of, throwError } from 'rxjs';

describe('MeterReadingUploadComponent', () => {
  let component: MeterReadingUploadComponent;
  let fixture: ComponentFixture<MeterReadingUploadComponent>;
  let meterReadingService: jasmine.SpyObj<MeterReadingService>;

  beforeEach(async () => {
    const meterReadingServiceSpy = jasmine.createSpyObj('MeterReadingService', ['uploadMeterReadings']);

    await TestBed.configureTestingModule({
      imports: [
        HttpClientTestingModule,
        MatIconModule,
        MatButtonModule
      ],
      declarations: [
        MeterReadingUploadComponent, 
        FileUploadComponent,
        ResponseDisplayComponent
      ],
      providers: [ { provide: MeterReadingService, useValue: meterReadingServiceSpy } ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(MeterReadingUploadComponent);
    component = fixture.componentInstance;
    meterReadingService = TestBed.inject(MeterReadingService) as jasmine.SpyObj<MeterReadingService>;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should set selectedFile if onFileSelected is called with file', () => {
    // Arrange
    const file = new File(['file'], 'file.csv');

    // Act
    component.onFileSelected(file);

    // Assert
    expect(component.selectedFile).toBe(file);
  });

  it('should set selectedFile to null if onFileSelected is called with null', () => {
    // Act
    component.onFileSelected(null);

    // Assert
    expect(component.selectedFile).toBeNull();
  });

  it('should set errorMessage if onUpload is called without file', () => {
    // Act
    component.onUpload();

    // Asset
    expect(component.errorMessage).toBe('Please select a file before uploading');
  });


  it('should call uploadMeterReadings and update response if onUpload is successful', () => {
    // Arrange
    const file = new File(['file'], 'file.csv');
    component.selectedFile = file;

    const response = { message: 'Upload successful', successfulReadings: 10, failedReadings: 0 };
    meterReadingService.uploadMeterReadings.and.returnValue(of(response));

    // Act
    component.onUpload();

    // Assert
    expect(meterReadingService.uploadMeterReadings).toHaveBeenCalledWith(file);
    expect(component.errorMessage).toBeNull();
    expect(component.response).toEqual(response);
  });

  it('should set errorMessage if onUpload failed', () => {
    // Arrange
    const file = new File(['file'], 'file.csv');
    component.selectedFile = file;

    const errorResponse = { error: { criticalError: 'Upload failed' } };
    meterReadingService.uploadMeterReadings.and.returnValue(throwError(errorResponse));

    // Act
    component.onUpload();

    // Assert
    expect(meterReadingService.uploadMeterReadings).toHaveBeenCalledWith(file);
    expect(component.response).toBeNull();
    expect(component.errorMessage).toBe('Upload failed');
  });

});
