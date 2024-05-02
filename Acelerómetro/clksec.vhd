LIBRARY ieee;
use IEEE.STD_LOGIC_1164.ALL;

entity clksec is
    Port ( clk_in : in STD_LOGIC;
           clk_out : buffer STD_LOGIC);
end clksec;

architecture Behavioral of clksec is
    signal counter : natural range 0 to 2_500_000 := 0; -- Asumiendo un reloj a 50 MHz
    signal clk_divided : STD_LOGIC := '0';

begin
    process(clk_in)
    begin
        if rising_edge(clk_in) then
            if counter = 2_500_000 then
                counter <= 0;
                clk_divided <= NOT clk_divided;
            else
                counter <= counter + 1;
            end if;
        end if;
    end process;

    clk_out <= clk_divided;
	 
end Behavioral;