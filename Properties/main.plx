# usage: perl vimj_cut.pl F_XXX
#   -> parse files to ./F_XXX/*

use strict;

$| = 1;

my $zero_char = chr(0);

my($file) = @ARGV;

my $file_idx = './'.$file.'.IDX';
my $file_bin = './'.$file.'.BIN';

my $dir = './'.$file;
mkdir($dir) unless (-d $dir);

open(FILE_IDX, $file_idx) || die "file error: $file_idx";	binmode(FILE_IDX);
open(FILE_BIN, $file_bin) || die "file error: $file_bin";	binmode(FILE_BIN);

my($file_name_base_addr,$file_cnt);

seek(FILE_BIN, 0x0C, 0);
$file_name_base_addr  = ord(getc(FILE_BIN));
$file_name_base_addr += ord(getc(FILE_BIN)) * 256;
$file_name_base_addr += ord(getc(FILE_BIN)) * 65536;
$file_name_base_addr += ord(getc(FILE_BIN)) * 16777216;

seek(FILE_IDX, 0x0C, 0);
$file_cnt  = ord(getc(FILE_IDX));
$file_cnt += ord(getc(FILE_IDX)) * 256;
$file_cnt += ord(getc(FILE_IDX)) * 65536;
$file_cnt += ord(getc(FILE_IDX)) * 16777216;

my($i,$j,$k,$l, $c,$c2, $data_offset,$data_size,$file_name_offset,$back_pos,$file_name, $buffer, $disp,$len,$dest,$from, @tmp,$r, $file1);

for($i=0; $i<$file_cnt; $i++){
	$data_offset  = ord(getc(FILE_IDX));
	$data_offset += ord(getc(FILE_IDX)) * 256;
	$data_offset += ord(getc(FILE_IDX)) * 65536;
	$data_offset += ord(getc(FILE_IDX)) * 16777216;
	
	$data_size  = ord(getc(FILE_IDX));
	$data_size += ord(getc(FILE_IDX)) * 256;
	$data_size += ord(getc(FILE_IDX)) * 65536;
	$data_size += ord(getc(FILE_IDX)) * 16777216;
	
	$file_name_offset  = ord(getc(FILE_IDX));
	$file_name_offset += ord(getc(FILE_IDX)) * 256;
	$file_name_offset += ord(getc(FILE_IDX)) * 65536;
	$file_name_offset += ord(getc(FILE_IDX)) * 16777216;
	
	$file_name = '';
	
	if($file_name_offset > 0){
		$file_name_offset += $file_name_base_addr;
		seek(FILE_BIN, $file_name_offset, 0);
		for(;;){
			$c = getc(FILE_BIN);
			last if ($c eq $zero_char);
			last if (eof(FILE_BIN));
			$file_name .= $c;
		}
	}
	else{
		$file_name = sprintf("%s_%04d.bin", $file, $i);
	}
	
	printf "%08X %08X %s", $data_offset,$data_size,$file_name;
	
	if($data_size < 1){
		print ' - skip.'."\n";
		next;
	}
	
	seek(FILE_BIN, $data_offset, 0);
	
	$buffer = '';
	
	$r = 0;
	@tmp = ();
	
	if($data_size & 0x10000000){
		$data_size = $data_size & 0xFFFFFFF;
		
		# header
		$c = ord(getc(FILE_BIN));
		$c = ord(getc(FILE_BIN));
		$c = ord(getc(FILE_BIN));
		$c = ord(getc(FILE_BIN));
		
		for($j=0; $j<$data_size; ){
			$c = ord(getc(FILE_BIN));	# read flag
			
			foreach $k (0..7){
				if($c & 0x80){	# Compressed
					$c2  = ord(getc(FILE_BIN)) * 256;
					$c2 += ord(getc(FILE_BIN));
					
					$disp = $c2 & 0xFFF;
					$len = ( ($c2 >> 12) & 0xF ) + 3;
					
					$from = $r - $disp - 1;
					$from += 0x1000 if $from < 0;
					
					foreach(1..$len){
						$buffer .= $tmp[$from];
						
						$tmp[$r++] = $tmp[$from];
						$r = $r & 0xFFF;
						
						$from++;
						$from = $from & 0xFFF;
					}
					
					$j += $len;
				}
				else{	# Uncompressed
					$c2 = getc(FILE_BIN);
					
					$buffer .= $c2;
					
					$tmp[$r++] = $c2;
					$r = $r & 0xFFF;
					
					$j++;
				}
				
				$c = $c << 1;
				last if ($j >= $data_size);
			}
			last if ($j >= $data_size);
		}
	}
	else{
		read(FILE_BIN,$buffer,$data_size);
	}
	
	$file1 = $dir.'/'.$file_name;
	open(FILE1, "> $file1") || die "file error: $file1";
	binmode(FILE1);
	print FILE1 $buffer;
	close(FILE1);
	
	print ' - OK!'."\n";
}