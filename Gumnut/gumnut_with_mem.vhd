library ieee;
use ieee.std_logic_1164.all, ieee.numeric_std.all;

entity gumnut_with_mem is
  generic ( IMem_file_name : string := "gasm_text.dat";
            DMem_file_name : string := "gasm_data.dat";
            debug : boolean := false );
  port ( clk_i : in std_logic;
         rst_i : in std_logic;
         -- I/O port bus
         port_cyc_o : out std_logic;
         port_stb_o : out std_logic;
         port_we_o : out std_logic;
         port_ack_i : in std_logic;
         port_adr_o : out unsigned(7 downto 0);
         port_dat_o : out std_logic_vector(7 downto 0);
         port_dat_i : in std_logic_vector(7 downto 0);
         -- Interrupts
         int_req : in std_logic;
         int_ack : out std_logic );
end entity gumnut_with_mem;
